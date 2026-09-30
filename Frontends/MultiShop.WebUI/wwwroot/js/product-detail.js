document.querySelector('.detail-review-link')?.addEventListener('click', () => {
    document.querySelector('.detail-tabs a[href="#tab-pane-3"]')?.click();
});
document.querySelectorAll('[data-quantity-change]').forEach(button => {
    button.addEventListener('click', () => {
        const input = button.parentElement.querySelector('input');
        input.value = Math.max(1, Math.min(99, (parseInt(input.value, 10) || 1) + Number(button.dataset.quantityChange)));
    });
});
