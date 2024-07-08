$(document).ready(function () {
    filterQuantity();
    $('body').on('click', '#add-to-card', function () {
        var sku = '';
        $('#tb_dataCTSP tbody tr').each(function () {
            if ($(this).find('td').length > 0) {
                sku = $(this).find('td:eq(0)').text().trim();
            }
        });
        
        var sl = $('#quantity_input').val();
        // Gửi yêu cầu AJAX tới controller
        $.ajax({
            url: '/CustomerHome/AddToCart', // Thay 'ControllerName' bằng tên controller của bạn
            type: 'POST',
            data: {
                maSPCT: sku,
                soluong: sl
            },
            success: function (response) {
                // Xử lý phản hồi từ server nếu cần
                alert('Sản phẩm đã được thêm vào giỏ hàng');
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi nếu có
                console.error('Đã xảy ra lỗi:', error);
            }
        });
    });
    $('#quantity_input').on('change', function () {
        var inputQuantity = parseInt($(this).val());
        var availableQuantity = parseInt($('#total').text());

        if (inputQuantity > availableQuantity) {
            $('#quantity-error').show();
            $(this).val(availableQuantity); // Đặt lại giá trị của input về số lượng tồn
        } else {
            $('#quantity-error').hide();
        }
    });

})

function filterQuantity() {
    var selectedColor = document.getElementById("group_2").value;
    var selectedSize = document.getElementById("group_1").value;
    var table = document.getElementById("tb_dataCTSP");
    var rows = table.getElementsByTagName("tbody")[0].getElementsByTagName("tr");

    var totalQuantity = 0;

    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var color = row.querySelector('[data-mamau]').getAttribute('data-mamau').trim();
        var size = row.querySelector('[data-masize]').getAttribute('data-masize').trim();
        var quantity = parseInt(row.querySelector('[data-soluong]').getAttribute('data-soluong').trim());

        if ((selectedColor === '' || color === selectedColor) &&
            (selectedSize === '' || size === selectedSize)) {
            row.style.display = "";
            totalQuantity += quantity;
        } else {
            row.style.display = "none";
        }
    }

    $('#total').text(totalQuantity + ' Sản Phẩm Còn Lại') ;
}
function getdataSPCT() {
    
    //$('#tb_dataCTSP tbody tr').each(function () {
    //    if ($(this).find('td').length > 0) {
    //         sku = $(this).find('td:eq(0)').text.trim();
    //    }
    //})
    //return sku;
}
