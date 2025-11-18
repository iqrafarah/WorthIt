        document.addEventListener("DOMContentLoaded", function () {
            const openModalBtn = document.getElementById("open-category-modal");
            const overlay = document.getElementById("category-modal-overlay");
            const categoryText = document.getElementById("selected-category-text");
            const categoryIcon = document.getElementById("category-icon-placeholder");
            const inputName = document.getElementById("CategoryName");
            const inputIcon = document.getElementById("CategoryIcon");

            if (!openModalBtn || !overlay) return;

            // open modal
            openModalBtn.addEventListener("click", function () {
                overlay.classList.remove("d-none");
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

                    categoryIcon.style.display = "none";
                    categoryText.textContent = icon + " " + name;
                    inputName.value = name;
                    inputIcon.value = icon;


                    overlay.style.display = "none";
                });
            });
        });