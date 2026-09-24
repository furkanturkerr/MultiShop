(() => {
    const form = document.getElementById('product-review-form');
    if (!form) return;

    const ratingGroup = form.querySelector('[data-review-rating]');
    const ratingInputs = [...ratingGroup.querySelectorAll('input[type="radio"]')];
    const updateRating = () => {
        const value = Number(ratingInputs.find(input => input.checked)?.value || 0);
        ratingInputs.forEach(input => {
            const label = input.closest('label');
            const icon = label.querySelector('i');
            const filled = Number(input.value) <= value;
            icon.classList.toggle('fas', filled);
            icon.classList.toggle('far', !filled);
            label.classList.toggle('is-selected', input.checked);
        });
        ratingGroup.querySelector('[data-rating-label]').textContent = value ? `${value} / 5` : 'Puan seç';
    };
    ratingInputs.forEach(input => input.addEventListener('change', updateRating));
    updateRating();

    const button = form.querySelector('[data-review-submit]');
    form.addEventListener('submit', event => {
        if (event.defaultPrevented || !form.checkValidity()) return;
        button.disabled = true;
        button.textContent = 'Gönderiliyor…';
    });
    window.addEventListener('pageshow', () => {
        button.disabled = false;
        button.textContent = 'Yorumu Gönder';
    });
})();
