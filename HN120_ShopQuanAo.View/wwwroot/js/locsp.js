function searchProduct() {
    var searchQuery = document.getElementById("searchInput").value;

    fetch(`/CustomerHome/GianHangNguoiDung?query=${encodeURIComponent(searchQuery)}`)
        .then(response => response.text())
        .then(data => {
            document.getElementById("searchResults").innerHTML = data;
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
