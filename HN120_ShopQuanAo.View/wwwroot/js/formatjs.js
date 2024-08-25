
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

    //document.addEventListener("change", function () {
    //    // Lấy tất cả các input số lượng sản phẩm
    //    const quantityInputs = document.querySelectorAll('input[type="number"]');

    //    quantityInputs.forEach(input => {
    //        input.addEventListener('change', function () {
    //            // Lấy số lượng mới và dòng chứa sản phẩm hiện tại
    //            const newQuantity = this.value;
    //            const row = this.closest('tr');

    //            // Lấy giá bán và cột thành tiền tương ứng
    //            const price = parseFloat(row.querySelector('.product-price[name="tien"]').innerText);
    //            const totalElement = row.getElementById('total');
             
    //            // Tính thành tiền mới
    //            const newTotal = newQuantity * price;
    //            totalElement.innerText = newTotal.toLocaleString('vi-VN');

    //            // Gửi dữ liệu về server bằng AJAX
    //            const productId = this.closest('a[asp-route-maSP]').getAttribute('asp-route-maSP');

    //            fetch('/Customer/CustomerHome/UpdateCart', {
    //                method: 'POST',
    //                headers: {
    //                    'Content-Type': 'application/json',
    //                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
    //                },
    //                body: JSON.stringify({
    //                    MaSP: productId,
    //                    SoLuong: newQuantity,
    //                    ThanhTien: newTotal
    //                })
    //            })
    //                .then(response => {
    //                    if (response.ok) {
    //                        return response.json();
    //                    } else {
    //                        throw new Error('Có lỗi xảy ra khi cập nhật giỏ hàng.');
    //                    }
    //                })
    //                .then(data => {
    //                    // Xử lý dữ liệu trả về từ server nếu cần
    //                    console.log('Giỏ hàng đã được cập nhật:', data);
    //                })
    //                .catch(error => {
    //                    console.error('Lỗi:', error);
    //                });
    //        });
    //    });
    //});

});

function formatMoney(amount) {
    if (!isNaN(amount) && amount !== null && amount !== '') {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
    } else {
        return amount;
    }
}
// format lại về float
function parseMoney(formattedAmount) {
    // Loại bỏ ký tự không phải số (bao gồm cả ký hiệu tiền tệ và dấu phân cách hàng nghìn)
    const numberString = formattedAmount.replace(/[^0-9.-]/g, '');
    // Chuyển đổi chuỗi thành số thực
    return parseFloat(numberString);
}
    document.addEventListener('DOMContentLoaded', function () {
        // Lấy tất cả các ô input số lượng
        const quantityInputs = document.querySelectorAll('.quantity-input');
        function calculateTotalAmount() {
            let totalAmount = 0;
            const totalCells = document.querySelectorAll('td[role="money"]');

            totalCells.forEach(cell => {
                console.log(cell.textContent);
                const amount = parseMoney(cell.textContent);
                if (!isNaN(amount)) {
                    totalAmount += amount;
                    console.log(totalAmount);
                }
            });

            // Cập nhật tổng tiền cần trả
            document.getElementById('amount_items').textContent = totalAmount;
        }
        // Duyệt qua tất cả các ô input và lắng nghe sự kiện input
        quantityInputs.forEach(input => {
        input.addEventListener('input', function () {
            // Lấy đơn giá từ thuộc tính data-don-gia
            const donGia = parseFloat(this.getAttribute('data-don-gia'));
            // Lấy số lượng từ giá trị hiện tại của ô input
            const quantity = parseInt(this.value);
            // Xác định phần tử hiển thị thành tiền tương ứng
            const totalCellId = 'total-' + this.id.split('-')[1];
            const totalCell = document.getElementById(totalCellId);

            // Kiểm tra nếu totalCell tồn tại
            if (totalCell) {
                // Tính toán thành tiền mới và cập nhật
                if (!isNaN(donGia) && !isNaN(quantity)) {
                    const thanhTien = donGia * quantity;
                    totalCell.textContent = formatMoney(thanhTien);
                } else {
                    totalCell.textContent = '0 đ';
                }
                calculateTotalAmount(); // Cập nhật tổng tiền mỗi khi số lượng thay đổi

            } else {
                console.error('Element with id ' + totalCellId + ' not found.');
            }
        });
        });
    });
