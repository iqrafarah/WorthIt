  document.addEventListener("DOMContentLoaded", function () {
        const openModalBtn = document.getElementById("open-category-modal");
        const overlay = document.getElementById("category-modal-overlay");
        const categoryText = document.getElementById("selected-category-text");
        const categoryIcon = document.getElementById("category-icon-placeholder");
        const inputName = document.getElementById("CategoryName");
        const inputIcon = document.getElementById("CategoryIcon");
        const categoryId = document.getElementById("categoryId");

        if (!openModalBtn || !overlay) return;

        // open modal
        openModalBtn.addEventListener("click", function () {
            overlay.classList.remove("d-none");
            overlay.style.display = "flex";
        });

        // close when clicking the dark background
        overlay.addEventListener("click", function (e) {
            if (e.target === overlay) {
                overlay.style.display = "none";
            }
        });

        categoryIcon.style.display = "block";

        // handle category click
        document.querySelectorAll(".category-option").forEach(btn => {
            btn.addEventListener("click", function () {
                const name = this.dataset.name;
                const icon = this.dataset.icon;
                const id = this.dataset.id;

                console.log(name, icon, id);

                categoryIcon.style.display = "none";
                categoryText.textContent = icon + " " + name;
                inputName.value = name;
                inputIcon.value = icon;
                categoryId.value = id;

                console.log("Category selected:", name, icon, id);

                overlay.style.display = "none";
            });
        });

        // add category modal
        const openAddCategoryModalBtn = document.getElementById("open-add-category-modal");
        const addCategoryOverlay = document.getElementById("add-category-modal-overlay");
        const closeAddCategoryModalBtn = document.getElementById("close-add-category-modal");

        if (openAddCategoryModalBtn && addCategoryOverlay) {
            openAddCategoryModalBtn.addEventListener("click", function () {
                addCategoryOverlay.classList.remove("d-none");
                addCategoryOverlay.style.display = "flex";
            });

            addCategoryOverlay.addEventListener("click", function (e) {
                if (e.target === addCategoryOverlay) {
                    addCategoryOverlay.style.display = "none";
                }
            });

            if (closeAddCategoryModalBtn) {
                closeAddCategoryModalBtn.addEventListener("click", function () {
                    addCategoryOverlay.style.display = "none";
                });
            }
        }
    });