(function () {
    const categories = JSON.parse(document.getElementById('categoryOptionSchemas').textContent);
    const select = document.getElementById('CategoryId');
    const container = document.getElementById('productOptions');
    if (!select || !container)
        return;

    const drafts = new Map();
    let currentCategoryId = select.value;

    function saveDraft() {
        const values = new Map();
        container.querySelectorAll('[data-option-name]').forEach(input => {
            values.set(input.dataset.optionName, input.value);
        });
        drafts.set(currentCategoryId, values);
    }

    function renderOptions() {
        const category = categories.find(x => x.CategoryId === currentCategoryId);
        const names = category ? category.OptionNames : [];
        const values = drafts.get(currentCategoryId) || new Map();
        container.replaceChildren();

        names.forEach((name, i) => {
            const field = document.createElement('div');
            field.className = 'ms-field';
            const label = document.createElement('label');
            label.htmlFor = `product-option-${i}`;
            label.textContent = name;
            const group = document.createElement('input');
            group.type = 'hidden';
            group.name = `Options[${i}].Name`;
            group.value = name;
            const input = document.createElement('input');
            input.id = label.htmlFor;
            input.className = 'ms-input';
            input.name = `Options[${i}].ValuesText`;
            input.dataset.optionName = name;
            input.value = values.get(name) || '';
            input.placeholder = 'Bu üründe bulunan değerler';
            field.append(label, group, input);
            container.appendChild(field);
        });

        if (names.length === 0) {
            const message = document.createElement('p');
            message.className = 'text-muted';
            message.textContent = category
                ? 'Bu kategoride seçenek grubu tanımlanmamış. Grupları kategori düzenleme ekranından ekleyebilirsin.'
                : 'Önce kategori seç.';
            container.appendChild(message);
        }
    }

    select.addEventListener('change', () => {
        saveDraft();
        currentCategoryId = select.value;
        renderOptions();
    });
})();
