
document.addEventListener("DOMContentLoaded", () => {
  const toast = document.querySelector("[data-toast]");

  function showToast(message) {
    if (!toast) return;
    const text = toast.querySelector("[data-toast-text]");
    if (text) text.textContent = message;
    toast.classList.add("show");
    clearTimeout(window.__msToastTimer);
    window.__msToastTimer = setTimeout(() => toast.classList.remove("show"), 2200);
  }

  // Product image live preview
  const imageInput = document.querySelector("[data-product-image-url]");
  const imagePreview = document.querySelector("[data-product-image-preview]");

  function updateImagePreview() {
    if (!imageInput || !imagePreview) return;

    const url = imageInput.value.trim();

    if (!url) {
      imagePreview.classList.remove("has-image");
      imagePreview.innerHTML = `
        <div class="ms-image-preview-inner">
          <i class="fa-regular fa-image"></i>
          <strong>Görsel önizleme</strong>
          <span>URL girdikten sonra ürün görseli burada görünür.</span>
        </div>`;
      return;
    }

    const img = new Image();
    img.alt = "Ürün görseli önizleme";
    img.onload = () => {
      imagePreview.classList.add("has-image");
      imagePreview.innerHTML = "";
      imagePreview.appendChild(img);
    };
    img.onerror = () => {
      imagePreview.classList.remove("has-image");
      imagePreview.innerHTML = `
        <div class="ms-image-preview-inner">
          <i class="fa-solid fa-triangle-exclamation"></i>
          <strong>Görsel yüklenemedi</strong>
          <span>URL adresini kontrol et.</span>
        </div>`;
    };
    img.src = url;
  }

  imageInput?.addEventListener("input", updateImagePreview);
  imageInput?.addEventListener("change", updateImagePreview);

  // Product attributes/options
  function wireRemoveButtons(container) {
    container?.querySelectorAll("[data-remove-item]").forEach(btn => {
      if (btn.dataset.bound === "1") return;
      btn.dataset.bound = "1";
      btn.addEventListener("click", () => btn.closest(".ms-option-item")?.remove());
    });
  }

  const attributeList = document.querySelector("[data-attribute-list]");
  const optionList = document.querySelector("[data-option-list]");

  wireRemoveButtons(attributeList);
  wireRemoveButtons(optionList);

  document.querySelector("[data-add-attribute]")?.addEventListener("click", (e) => {
    e.preventDefault();
    attributeList?.insertAdjacentHTML("beforeend", `
      <div class="ms-option-item">
        <input class="ms-input" name="attributeName[]" placeholder="Özellik adı">
        <input class="ms-input" name="attributeValue[]" placeholder="Değer">
        <button class="ms-option-remove" data-remove-item type="button" aria-label="Özelliği sil">
          <i class="fa-regular fa-trash-can"></i>
        </button>
      </div>`);
    wireRemoveButtons(attributeList);
    attributeList?.lastElementChild?.querySelector("input")?.focus();
  });

  document.querySelector("[data-add-option]")?.addEventListener("click", (e) => {
    e.preventDefault();
    optionList?.insertAdjacentHTML("beforeend", `
      <div class="ms-option-item">
        <input class="ms-input" name="optionName[]" placeholder="Seçenek adı (örn. Renk)">
        <input class="ms-input" name="optionValues[]" placeholder="Değerleri virgülle ayır (örn. Siyah, Beyaz)">
        <button class="ms-option-remove" data-remove-item type="button" aria-label="Seçenek grubunu sil">
          <i class="fa-regular fa-trash-can"></i>
        </button>
      </div>`);
    wireRemoveButtons(optionList);
    optionList?.lastElementChild?.querySelector("input")?.focus();
  });

  // Category slug + live preview
  const categoryName = document.querySelector("[data-category-name]");
  const categorySlug = document.querySelector("[data-category-slug]");
  const previewName = document.querySelector("[data-preview-name]");
  const previewSlug = document.querySelector("[data-preview-slug]");
  const previewChips = document.querySelector("[data-preview-chips]");
  let slugManuallyEdited = false;

  function slugify(value) {
    return value
      .toLocaleLowerCase("tr-TR")
      .replace(/ğ/g, "g")
      .replace(/ü/g, "u")
      .replace(/ş/g, "s")
      .replace(/ı/g, "i")
      .replace(/ö/g, "o")
      .replace(/ç/g, "c")
      .normalize("NFD")
      .replace(/[\u0300-\u036f]/g, "")
      .replace(/[^a-z0-9]+/g, "-")
      .replace(/^-+|-+$/g, "");
  }

  function updateCategoryPreview() {
    if (previewName && categoryName) {
      previewName.textContent = categoryName.value.trim() || "Yeni Kategori";
    }
    if (previewSlug && categorySlug) {
      previewSlug.textContent = categorySlug.value.trim() || "kategori-slug";
    }
  }

  categoryName?.addEventListener("input", () => {
    if (categorySlug && !slugManuallyEdited) {
      categorySlug.value = slugify(categoryName.value);
    }
    updateCategoryPreview();
  });

  categorySlug?.addEventListener("input", () => {
    slugManuallyEdited = categorySlug.value.trim().length > 0;
    updateCategoryPreview();
  });

  document.querySelectorAll("[data-option-template]").forEach(cb => {
    cb.addEventListener("change", () => {
      if (!previewChips) return;
      const selected = [...document.querySelectorAll("[data-option-template]:checked")]
        .map(x => x.value);

      previewChips.innerHTML = selected.length
        ? selected.map(x => `<span>${x}</span>`).join("")
        : `<span>Seçenek yok</span>`;
    });
  });

  // Demo form submit (static theme)
  document.querySelectorAll("[data-demo-form]").forEach(form => {
    form.addEventListener("submit", e => {
      e.preventDefault();
      showToast(form.dataset.successMessage || "Değişiklikler kaydedildi.");
    });
  });
});
