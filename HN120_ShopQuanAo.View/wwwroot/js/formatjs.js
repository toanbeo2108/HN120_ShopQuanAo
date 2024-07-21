
// Duyệt qua các ô chứa số tiền và định dạng
$(document).ready(function () {

    document.getElementById('updateCartButton').addEventListener('click', function () {
        // Create a new AJAX request
        var xhr = new XMLHttpRequest();

        // Configure it: GET-request for the URL /Customer/CustomerHome/UpdateGioHang
        xhr.open('POST', '/Customer/CustomerHome/UpdateGioHang', true);

        // Set up the callback to handle the response
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                // Request finished and response is ready
                // You can process the response here, e.g., update the cart display
                alert('Cart updated successfully!');
            }
        };

        // Send the request
        xhr.send();
    });
    var moneyElements = document.getElementsByName('tien');

    // Duyệt qua từng phần tử và format giá trị tiền tệ
    $.each(moneyElements, function (index, element) {
        var $element = $(element);
        var amountText = $element.text().trim(); // Lấy số tiền từ văn bản và loại bỏ khoảng trắng

        // Xử lý số tiền từ văn bản
        var amountWithoutCommas = amountText.split(',')[0].replace(/,/g, ''); // Bỏ dấu phẩy và phần sau dấu phẩy
        var amount = parseFloat(amountWithoutCommas);

        // Định dạng số tiền và gán lại vào văn bản nếu số tiền hợp lệ
        if (!isNaN(amount)) {
            $element.text(formatMoney(amount));
        }
    });
});

function formatMoney(amount) {
    if (!isNaN(amount) && amount !== null && amount !== '') {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
    } else {
        return amount;
    }
}
