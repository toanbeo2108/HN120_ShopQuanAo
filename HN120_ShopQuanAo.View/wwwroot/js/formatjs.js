
// Duyệt qua các ô chứa số tiền và định dạng
$(document).ready(function () {

    var tongtien = $('#amount_items').val();
    var tienship = $('#ship_amount').val();
    var tongdon = $('#total_amount').val();
    $('#amount_items').val(formatMoney(tongtien));
    $('#ship_amount').val(formatMoney(tienship));
    $('#total_amount').val(formatMoney(tongdon));

    document.getElementById('updateCartButton').addEventListener('change', function () {
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
function parseMoneyToFloat(formattedAmount) {
    // Bỏ ký tự không phải số hoặc dấu thập phân (loại bỏ ₫ và dấu chấm ngăn cách hàng nghìn)
    const cleanedString = formattedAmount.replace(/\./g, '').replace(/[^0-9,]/g, '');
    // Đổi dấu phẩy (ngăn cách hàng thập phân) thành dấu chấm
    const normalizedString = cleanedString.replace(',', '.');
    // Chuyển chuỗi thành số thực (float)
    return parseFloat(normalizedString);
}



//document.addEventListener('input', function () {
//    const quantityInputs = document.querySelectorAll('.quantity-input');
    
//});

    document.addEventListener('DOMContentLoaded', function () {
        // Lấy tất cả các ô input số lượng
        const quantityInputs = document.querySelectorAll('.quantity-input');
        quantityInputs.forEach(input => {
            input.addEventListener('input', function () {
                // Lấy đơn giá từ thuộc tính data-don-gia

                let value = input.value;
                // Kiểm tra nếu giá trị rỗng hoặc không phải là số
                console.log(value);
                if (value === '' || isNaN(value)) {
                    input.value = 1;
                    location.reload();
                } else {
                    // Chuyển giá trị về dạng số nguyên để so sánh
                    value = parseInt(value, 10);

                    // Kiểm tra nếu giá trị nhỏ hơn 1
                    if (value < 1) {
                        input.value = 1;
                    }

                    // Kiểm tra nếu giá trị lớn hơn 100
                    if (value > 100) {
                        input.value = 100;
                    }
                }
            });
        });
        function calculateTotalAmount() {
            let totalAmountItem = 0;
            const totalCells = document.querySelectorAll('td[role="money"]');

            totalCells.forEach(cell => {
                console.log(cell.textContent);
                const amount = parseMoneyToFloat(cell.textContent);
                console.log(amount);
                if (!isNaN(amount)) {
                    totalAmountItem += amount;
                }
            });

            // Cập nhật tổng tiền cần trả
            $('#amount_items').val(formatMoney(totalAmountItem)) ;
            $('#total_amount').val(formatMoney(totalAmountItem)) ;
        }
        // gửi ajax về controller
        function sendQuantityToServer(maGHCT, quantity) {
            fetch(`/api/GioHangChiTiet/UpdateGHCT/${maGHCT}?soluong=${quantity}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    if (data === true) {

                    } else if (quantity <= 1) {
                        console.error('Số lượng không được nhỏ hơn 1');
                        alert('Số lượng không được nhỏ hơn 1');
                    }
                    else {
                        alert('Lỗi không thể thay đổi thông tin');
                    }
                })
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
            const productIdCell = this.closest('tr').querySelector('td');
            const maGHCT = productIdCell.textContent.trim();
            if (totalCell) {
                // Tính toán thành tiền mới và cập nhật
                if (!isNaN(donGia) && !isNaN(quantity)) {
                    const thanhTien = donGia * quantity;
                    totalCell.textContent = formatMoney(thanhTien);
                } else {
                    totalCell.textContent = '0 đ';
                }
                calculateTotalAmount(); // Cập nhật tổng tiền mỗi khi số lượng thay đổi
                // gửi về  controller
                sendQuantityToServer(maGHCT, quantity);
            } else {
                console.error('Element with id ' + totalCellId + ' not found.');
            }
        });
        });
    });
