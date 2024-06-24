$(document).ready(function () {
    $('#pop').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Button that triggered the modal
        var dataId = button.data('id'); // Extract info from data-* attributes
        var modal = $(this);
        modal.find('#btn_ma').val(dataId); // Assign the data-id to the input field
    });
    $('body').on('click', '#btn_updatedhct', function () {

        var btn = parseFloat($(this).attr('data-GiaBan'));
       // var sl = parseFloat($('#soluong').val());
        var ten = $(this).attr('data-tensp');
        var sku = $(this).attr('data-SKU');
        var img = $(this).attr('data-img');
        let mahoadon = $('#btn_mahoadon').val();
        $('#pop2').modal('show');
        $('#pop_sp').modal('hide');
        $('#btn_themsoluong').off('click').on('click', function () {
            let soluong = parseFloat($('#inp_soluongmua').val());
            let dg = soluong * btn;
            var data_ = {
                //MaHoaDonChiTiet: ma,
                UrlAnhSpct: img,
                SKU: sku,
                MaHoaDon: mahoadon,
                TenSp: ten,
                SoLuongMua: soluong,
                DonGia: dg,

            };
            $.ajax({
                url: '/Add-hoadonct',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify([data_]),
                success: function (re) {
                    if (re.status) {
                        alert(re.message);
                        $('#pop2').modal('hide');
                        window.location.reload();
                    } else {
                        alert(re.message);
                    }
                },
                error: function (error) {
                    alert('Có lỗi xảy ra khi gửi dữ liệu.');
                }
            });
        })
    });
    $('body').on('click', '#btn_dell', function () {
        var ma = $('#in_ma').val();
        $.post('/Dell-HDCT/' + ma, function (re) {
            if (re.status) {
                alert(re.message)
                window.location.reload();
            }
        })
    })
    $('body').on('click', '#btn_pop_sp', function () {
        $('#pop_sp').modal('show');
    })
    $('body').on('click', '#btn_popxoa', function () {
        let ma = $(this).attr('data-id')
        $('#in_ma').val(ma)
        $('#pop').modal('show');
    })

})
