(function ($) {
    'use strict';

    function formatPrice(value) {
        return Number(value).toLocaleString('tr-TR', {
            minimumFractionDigits: 0,
            maximumFractionDigits: 2
        }) + ' ₺';
    }

    $(function () {
        var config = $('#cartAjaxConfig');
        if (!config.length) {
            return;
        }

        $('.js-cart-quantity').on('click', function () {
            var button = $(this);
            var row = button.closest('[data-cart-row]');
            var quantityInput = row.find('.js-cart-quantity-value');
            var currentQuantity = parseInt(quantityInput.val(), 10);
            var requestedQuantity = currentQuantity + parseInt(button.data('change'), 10);

            if (requestedQuantity < 1 || requestedQuantity > 99) {
                return;
            }

            var rowButtons = row.find('.js-cart-quantity');
            var errorBox = $('.js-cart-error');
            rowButtons.prop('disabled', true);
            errorBox.addClass('d-none').text('');

            $.ajax({
                url: config.data('update-url'),
                type: 'POST',
                dataType: 'json',
                data: {
                    productId: button.data('product-id'),
                    quantity: requestedQuantity,
                    __RequestVerificationToken: $('#cartAjaxTokenForm input[name="__RequestVerificationToken"]').val()
                }
            }).done(function (response) {
                quantityInput.val(response.quantity);
                row.find('.js-cart-line-total').text(formatPrice(response.lineTotal));
                $('.js-cart-subtotal').text(formatPrice(response.subtotal));
                $('.js-cart-discount').text('-' + formatPrice(response.discountAmount));
                $('.js-cart-total').text(formatPrice(response.total));
            }).fail(function (xhr) {
                var message = xhr.responseJSON && xhr.responseJSON.message
                    ? xhr.responseJSON.message
                    : 'Ürün adedi güncellenemedi. Lütfen tekrar deneyin.';

                errorBox.removeClass('d-none').text(message);
            }).always(function () {
                rowButtons.prop('disabled', false);
                row.find('.js-cart-quantity[data-change="-1"]')
                    .prop('disabled', parseInt(quantityInput.val(), 10) <= 1);
            });
        });
    });
})(jQuery);
