$(document).ready(function () {

    $('body').on('click', '#btn_add', function () {

        setdata(null);
        $('#pop').modal('show');
    })
    $('body').on('click', '#btn_chitiet', function () {
        let ma = $(this).attr('data-id');
        $.get('/thanhtoan-detail/' + ma, function (re) {
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

        let id = $('#btn_mapt').val();
        if ($('#btn_mapt').val() == null || $('#btn_mapt').val() == undefined || $('#btn_mapt').val() == '') {
            $.post('/Add-ThanhToan', { tt: getdata() }, function (re) {

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
            $.post('/Update-thanhtoan/', { tt: getdata() }, function (re) {

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
        $('#btn_mapt').val('');
        $('#btn_tenpt').val('');
        $('#btn_ngaytao').val(moment().format('YYYY-MM-DD'));
        $('#btn_Ngaythaydoi').val(moment().format('YYYY-MM-DD'));
        $('#btn_mota').val('');
        $('#btn_Status').val('');
    }
    else {
        var ngt = moment(data.ngayTao).format('YYYY-MM-DD');
        var ntd = moment(data.ngayThayDoi).format('YYYY-MM-DD');
        $('#btn_mapt').val(data.maPhuongThuc);
        $('#btn_tenpt').val(data.tenPhuongThuc);
        $('#btn_ngaytao').val(data.ngayTao) != null ? $('#btn_ngaytao').val(ngt) : '';
        $('#btn_Ngaythaydoi').val(data.ngayThayDoi) != null ? $('#btn_Ngaythaydoi').val(ntd) : '';
        $('#btn_mota').val(data.moTa);
        $('#btn_Status').val(data.trangThai);
    }
}
function getdata() {
    return {
        MaPhuongThuc: $('#btn_mapt').val(),
        TenPhuongThuc: $('#btn_tenpt').val(),
        MoTa: $('#btn_mota').val(),
        NgayTao: $('#btn_ngaytao').val(),
        NgayThayDoi: $('#btn_Ngaythaydoi').val(),
        TrangThai: $('#btn_Status').val()
    }
}
