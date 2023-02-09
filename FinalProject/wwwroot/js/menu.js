const body = document.querySelector("body"),
    sidebar = body.querySelector(".sidebar"),
    toggle = sidebar.querySelector(".toggle"),
    searchBtn = sidebar.querySelector(".search-box")

toggle.addEventListener("click", () => {
    sidebar.classList.toggle("close");
});

searchBtn.addEventListener("click", () => {
    sidebar.classList.remove("close");
});