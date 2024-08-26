$(document).ready(function () {
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


    $('.cancel-button').click(function () {
        var maHD = $(this).data('id');
        console.log("Delete button clicked for ID: " + maHD);
        if (maHD != null) {
            Swal.fire({
                title: 'Hủy Đơn?',
                text: "Bạn Có thực sự muốn hủy đơn hàng này không ?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Có, hủy đơn'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'GET',
                        url: ' /Customer/HoaDonCustomer/HuyDon',
                        data: { maHD: maHD },
                        success: function (response) {
                            console.log("Delete response received", response);
                            if (response.success) {
                                Swal.fire(
                                    'Success!',
                                    'Đơn Hàng Của Bạn đã được hủy',
                                    'success'
                                ).then(() => {
                                    location.reload();
                                });
                            } else {
                                Swal.fire(
                                    'Error!',
                                    'Không thể hủy đơn do đơn hàng đã được thay đổi bởi người bán',
                                    'error'
                                );
                            }
                        },
                        error: function () {
                            console.log("Delete AJAX error occurred");
                            Swal.fire(
                                'Error!',
                                'Không thể hủy đơn do đơn hàng đã được thay đổi bởi người bán',
                                'error'
                            );
                        }
                    });
                }
            });
        };

    });
});
function formatMoney(amount) {
    if (!isNaN(amount) && amount !== null && amount !== '') {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
    } else {
        return amount;
    }
}