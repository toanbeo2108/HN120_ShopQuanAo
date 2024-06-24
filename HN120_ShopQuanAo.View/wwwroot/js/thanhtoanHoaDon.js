
$(document).ready(function () {
    $('body').on('click', '#btn_add', function () {

        setdata(null);
        $('#pop').modal('show');
    })
    $('body').on('click', '#btn_chitiet', function () {
        let ma = $(this).attr('data-id');
        $.get('/thanhtoanhoadon-detail/'+ma, function (re) {
            if (re.status) {
                setdata(re.data)
                $('#pop').modal('show');
            }
            else {
                alert('Không tìm thấy')
            }
        })
    })

    $('body').on('click', '#btn_save', function () {

        let ma = $('#btn_mapthd').val();
        if ($('#btn_mapthd').val() == null || $('#btn_mapthd').val() == undefined || $('#btn_mapthd').val() == '') {
            $.post('/Add-ThanhToanhoadon', { tt: getdata() }, function (re) {

                if (re.status) {
                    alert(re.message)
                    $('#pop').modal('hide');
                    window.location.reload();
                }
                else {
                    alert(re.message);
                }
            })
        }
        else {
            $.post('/Update-thanhtoanhoadon/', { tt: getdata() }, function (re) {

                if (re.status) {
                    alert(re.message)
                    $('#pop').modal('hide');
                    window.location.reload();
                }
                else {
                    alert(re.message)
                }
            })
        }
    })

})
function setdata(data) {
    if (data == null || data == undefined || data == '') {
        $('#btn_mapthd').val('');
        $('#btn_mahd').val('');
        $('#btn_MaPhuongThuc').val('');
        $('#btn_ngaytao').val(moment().format('YYYY-MM-DD')); 
        $('#btn_Ngaythaydoi').val(moment().format('YYYY-MM-DD'));
        $('#btn_mota').val('');
        $('#btn_Status').val('');
    }
    else {
        var ngt = moment(data.ngayTao).format('YYYY-MM-DD');
        var ntd = moment(data.ngayThayDoi).format('YYYY-MM-DD');
        $('#btn_mapthd').val(data.maPhuongThuc_HoaDon);
        $('#btn_mahd').val(data.maHoaDon);
        $('#btn_MaPhuongThuc').val(data.maPhuongThuc);
        $('#btn_ngaytao').val(data.ngayTao) != null ? $('#btn_ngaytao').val(ngt) : '';
        $('#btn_Ngaythaydoi').val(data.ngayThayDoi) != null ? $('#btn_Ngaythaydoi').val(ntd) : '';
        $('#btn_mota').val(data.moTa);
        $('#btn_Status').val(data.trangThai);
    }
}
function getdata() {
    return {
        MaPhuongThuc_HoaDon: $('#btn_mapthd').val(),
        MaHoaDon: $('#btn_mahd').val(),
        MaPhuongThuc: $('#btn_MaPhuongThuc').val(),
        MoTa: $('#btn_mota').val(),
        NgayTao: $('#btn_ngaytao').val(),
        NgayThayDoi:$('#btn_Ngaythaydoi').val(),
        TrangThai: $('#btn_Status').val()
    }
}
