//$(document).ready(function () {

//    $('body').on('click', '#btn_add', function () {
//        setdata(null);
//        $('#pop').modal('show');

//    })
//    $('body').on('click', '#btn_chitiet', function () {
//        let ma = $(this).attr('data-id')
//        $.get('/Detail-hoadonCT/' + ma, function (re) {
//            if (re.status) {
//                setdata(re.data)
//                $('#pop').modal('show');
//            }
//            else {
//                alert(re.message)
//            }
//        })
//    })
//    $('body').on('click', '#btn_save', function () {


//        if ($('#btn_ma').val() == null || $('#btn_ma').val() == undefined || $('#btn_ma').val() == '') {
//            $.post('/Add-hoadonct', { hd: getdata() }, function (re) {

//                if (re.status) {
//                    alert(re.message)
//                    $('#pop').modal('hide');
//                    window.location.reload();
//                }
//                else {
//                    alert(re.message);
//                }
//            })
//        }
//        else {
//            $.post('/Update-hoadonct', { hd: getdata() }, function (re) {

//                if (re.status) {
//                    alert(re.message)
//                    $('#pop').modal('hide');
//                    window.location.reload();
//                }
//                else {
//                    alert(re.message)
//                }
//            })
//        }
//    })

//})
//function setdata(data) {
//    if (data == null || data == undefined || data == '') {
//        $('#btn_ma').val('');
//        $('#btn_SKU').val('');
//        $('#btn_MaHoaDon').val('');
//        $('#btn_TenSp').val('');
//        $('#btn_DonGia').val('');
//        $('#btn_SoLuongMua').val('');
//        $('#btn_Status').val('');
//    }
//    else {
//        $('#btn_ma').val(data.maHoaDonChiTiet);
//        $('#btn_SKU').val(data.sku);
//        $('#btn_MaHoaDon').val(data.maHoaDon);
//        $('#btn_TenSp').val(data.tenSp);
//        $('#btn_DonGia').val(data.donGia);
//        $('#btn_SoLuongMua').val(data.soLuongMua);
//        $('#btn_Status').val(data.trangThai);
//    }
//}
//function getdata() {
//    return {
//       MaHoaDonChiTiet: $('#btn_ma').val(),
//       SKU: $('#btn_SKU').val(),
//       MaHoaDon: $('#btn_MaHoaDon').val(),
//       TenSp: $('#btn_TenSp').val(),
//       DonGia: $('#btn_DonGia').val(),
//       SoLuongMua: $('#btn_SoLuongMua').val(),
//       TrangThai: $('#btn_Status').val(5)        
//    }                                           
//}
