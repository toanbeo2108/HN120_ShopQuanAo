//import { Console } from "console";

$(document).ready(function () {
    var notification = localStorage.getItem('notification');
    if (notification) {
        notification = JSON.parse(notification);
        $.notify(notification.message, notification.type);
        localStorage.removeItem('notification');
    } 
    $('#thanhtoantaiquay').show();
    $('#btn_khachletaiquay').show();
   
    var Parameter = {
        url: "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json",
        method: "GET",
        responseType: "application/json",
    };
    var promise = axios(Parameter);
    promise.then(function (result) {
        renderCity(result.data);
    });

    $('body').on('click', '#clear', function () {

        document.getElementById('city').innerHTML = '<option value="" selected>Chọn tỉnh thành</option>';
        document.getElementById('district').innerHTML = '<option value="" selected>Chọn quận huyện</option>';
        document.getElementById('ward').innerHTML = '<option value="" selected>Chọn phường xã</option>';
        document.getElementById('street').value = '';
        $('#btn_PhiShip').val(0);
        $('#btn_PhiShip_fake').val('');
        $('#ngnhanhang_btn').val('');
        $('#sdtnhanhang_btn').val('');
        updatePaymentDetails();
        var Parameter = {
            url: "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json",
            method: "GET",
            responseType: "application/json",
        };
        var promise = axios(Parameter);
        promise.then(function (result) {
            renderCity(result.data);
        });
    })

    $('#btn_giaohang').change(function () {
        var isChecked = $(this).is(':checked');
        if (isChecked) {
            $('#btn_chondiachi').show();
            $('#dathangtaiquay').show();
            $('#thanhtoantaiquay').hide();
            $('.cell_tienphip').show();
            $('#btn_khachletaiquay').hide();
            var tenkh = $('#search_khachhang_input')
           // var selectedOption = tenkh.attr('option:selected');
            var tinhThanh = tenkh.attr('data-tinh');
            var quanHuyen = tenkh.attr('data-quanhuyen');
            var xaPhuong = tenkh.attr('data-xaphuong');
            var cuThe = tenkh.attr('data-cuthe');
            var sodt = tenkh.attr('data-sodienthoai');
          
            var sdtnhanhhang = tenkh.attr('data-sdtnhanhhang');
            var ngnhanhang = tenkh.attr('data-ngnhanhang');

            document.getElementById('btn_SoDienThoai').value = sodt;
            document.getElementById('sdtnhanhang_btn').value = sdtnhanhhang;
            document.getElementById('ngnhanhang_btn').value = ngnhanhang;

            if ($('#search_khachhang_input').val() != '' || $('#search_khachhang_input').val() != undefined) {

                document.getElementById('city').innerHTML = '<option value="' + tinhThanh + '">' + tinhThanh + '</option>';
                document.getElementById('district').innerHTML = '<option value="' + quanHuyen + '">' + quanHuyen + '</option>';
                document.getElementById('ward').innerHTML = '<option value="' + xaPhuong + '">' + xaPhuong + '</option>';
                document.getElementById('street').value = cuThe;
                document.getElementById('btn_SoDienThoai').value = sodt;
                document.getElementById('sdtnhanhang_btn').value = sdtnhanhhang;
                document.getElementById('ngnhanhang_btn').value = ngnhanhang;
                loadTinhThanh();
            }
            if (tinhThanh == undefined && quanHuyen == undefined && xaPhuong == undefined && cuThe == undefined) {
                $('#clear').click();
            }
            
        } else {
            $('#btn_chondiachi').hide();
            $('#dathangtaiquay').hide();
            $('#thanhtoantaiquay').show();
            $('#btn_khachletaiquay').show();
          
            $('.cell_tienphip').hide();
            $('#search_khachhang_input').val('');
            $('#btn_SoDienThoai_').val('');
            
            $('#btn_fullname').val('');
            
            $('#sdtnhanhang_btn').val('');
            $('#ngnhanhang_btn').val('');
            $('#btn_Email').val('');
            $('#btn_Password').val('');
            $('#btn_Password').val('');

            let input = document.getElementById('search_khachhang_input');
            input.setAttribute('data-idKH', '');
            input.setAttribute('data-tinh', '');
            input.setAttribute('data-quanhuyen', '');
            input.setAttribute('data-xaphuong', '');
            input.setAttribute('data-cuthe', '');
            input.setAttribute('data-sdtnhanhhang', '');
            input.setAttribute('data-ngnhanhang', '');

            $('#clear').click();
        }
    })
    //  var token = '8fbfedf6-b458-11ee-b6f7-7a81157ff3b1';
    var token = 'd01771f0-3f8b-11ef-8f55-4ee3d82283af';
    filterFunction();
    addSelectButtonEventListeners();
    $('#btn_SoDienThoai').on('change', validatePhoneNumber);
    var today = new Date();

    var date = 'HD' + today.getDate() + (today.getMonth() + 1) + today.getFullYear() + today.getHours() + today.getMinutes() + today.getSeconds();
    $('#btn_ma').val(date.toString());
    $('#sanpham_table tbody tr').each(function () {
        var giaBanCell = $(this).find('td:eq(3)');

        var giaBanText = giaBanCell.html().split('<br>')[0].trim();
        var giaBan = parseFloat(giaBanText.replace(' VNĐ', '').replace(/\./g, '').replace(',', '.'));
      //  var giaBan = parseFloat(giaBanCell.text().trim().replace(/\./g, '').replace(',', '.'));
        if (!isNaN(giaBan)) {
            // giaBanCell.text(formatMoney(giaBan));
            giaBanCell.html(formatMoney(giaBan) + ' VNĐ<br>' + giaBanCell.html().split('<br>')[1]);
        }
    });
    $('#btn_NgayTaoDon').val(moment().format('YYYY-MM-DD HH:mm:ss'));
    $('#btn_Update').val(moment().format('YYYY-MM-DD HH:mm:ss'));
    $('body').on('click', '#btn_thanhtoanhoadon', function () {
        if ($('#btn_phuongthucthanhtoan').val() == '4') {

            $.notify('Phương thức thanh toán chỉ áp dụng cho đặt hàng tại quầy', 'error');
            return;
        }
        let tongtiensp = $('#btn_tongtien').val();
        if (tongtiensp == '' || parseFloat(tongtiensp) <= 0) {
            $.notify('Chưa chọn sản phẩm nào ', 'error');
            return;
        }
        var today = new Date();
        var date = 'HD' + today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        $('#btn_ma').val(date.toString());
        $('#btn_Status').val(5);

        let tt = ($('#btn_tienkhachdua').val().replace(/\./g, '').replace(',', '.'));
        let kt = ($('#btn_tienkhachphaitra').val().replace(/\./g, '').replace(',', '.'));
       
        if ((tt - kt >= 0) || tt == '' || tt == '' || tt == 0) {
            if ($('#btn_phuongthucthanhtoan').val() =='1') {
               
                Thanhtoan();
                AddLichsuhoadon();
                
            }
            if ($('#btn_phuongthucthanhtoan').val() == '2') {
              
                QRCODE_PAYMENT();
               
            }
                    
        }
        else {
            $.notify('Tiền khách đưa chưa đủ', 'error')
        }
        if ( $('#btn_phuongthucthanhtoan').val() == '3') {

            QRCODE_PAYMENT();
           
        }
       
    });
    async function loadTinhThanh() {
        var tinhthanh = $('#city').val();
        let provinceID;
        let districtID;
        let wardCode ;
        try {
            const response = await $.ajax({
                url: 'https://online-gateway.ghn.vn/shiip/public-api/master-data/province',
                method: 'GET',
                headers: {
                    'Token': token
                }
            });

            response.data.forEach(function (item) {
                if (item.ProvinceName.trim() === tinhthanh.replace(/^Tỉnh\s+|\s+$/g, '').trim()) { // So sánh chuỗi đã loại bỏ khoảng trắng thừa và dấu tiếng Việt
                    provinceID = item.ProvinceID;
                }
            });
            await loadQuanHuyen(provinceID)
        } catch (error) {
            alert('Không tìm thấy mã tỉnh.');
        }
        async function loadQuanHuyen(provinceId) {
            var quanhuyen = $('#district').val();
            try {
                const response = await $.ajax({
                    url: 'https://online-gateway.ghn.vn/shiip/public-api/master-data/district',
                    method: 'GET',
                    headers: {
                        'Token': token
                    }
                });

                response.data.forEach(function (item) {
                    if (item.DistrictName.trim() === quanhuyen.trim()) {

                        districtID = item.DistrictID;
                    }

                });
                await loadXaPhuong(districtID);
            } catch (error) {
                handleError(error);
            }
        }
        async function loadXaPhuong(districtID) {
            var xaphuong = $('#ward').val();
            try {
                const response = await $.ajax({
                    url: 'https://online-gateway.ghn.vn/shiip/public-api/master-data/ward',
                    method: 'GET',
                    headers: {
                        'Token': token
                    },
                    data: { district_id: districtID }
                });

                response.data.forEach(function (item) {
                    if (item.WardName.trim() === xaphuong.trim()) {

                        wardCode = item.WardCode;
                    }
                });
                //if (wardCode == undefined) {
                //  $.notify('Hiện giao hàng nhanh chưa hỗ trợ địa điểm này, thông cảm !', 'error') 
                //}
                console.log('Mã Tỉnh' + provinceID + 'Mã quận: ' + districtID + 'Mã xã: ' + wardCode);
                await getShippingFee(provinceID, districtID, wardCode)
            } catch (error) {

            }
        }
        async function getShippingFee(provinceID, districtID, wardCode) {
            const SshopId = 192652;
            try {
                const response = await $.ajax({
                    url: 'https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Token': token
                        //'ShopId': SshopId
                    },
                    data: JSON.stringify({
                        "service_type_id": 2,
                        "from_district_id": 3440,
                        "from_ward_code": "13009",
                        "to_district_id": districtID,
                        "to_ward_code": wardCode,
                        "height": 20,
                        "length": 30,
                        "weight": 3000,
                        "width": 40,
                        "insurance_value": 0,
                        "coupon": null

                    }) 
                });
           
                if (response.code === 200) {
                  
                    $('#btn_PhiShip').val(response.data.total);
                    var ship = parseFloat(response.data.total);
                    $('#btn_PhiShip_fake').val(formatMoney(ship)) ;
                    updatePaymentDetails()
                    
                } else {
                    console.error('Error:', response.message);
                }
            } catch (error) {
                console.error('Error calling API:', error);
            }
        }
    }

    $('#ward').change(function () {
        if ($('#btn_giaohang').is(':checked') == true) {
           
            loadTinhThanh();
            
        }
        else {
            $('#btn_PhiShip').val(0);
        }
       
    })
    $('#btn_MaVoucher').change(function () {       
        updatePaymentDetails();  
    });
    $('#btn_PhiShip').change(function () {
        updatePaymentDetails();
    });
    $('#btn_MaVoucher').change(updatePaymentDetails);
    $('#btn_PhiShip').on('input', updatePaymentDetails);
    $('#btn_chonsp').click(updatePaymentDetails);

    // Lắng nghe thay đổi số lượng sản phẩm trong bảng chi tiết hóa đơn (giả sử có input số lượng)
    $('#chitiethoadon_table').on('input', 'input[type="number"]', updatePaymentDetails);
    //formatPriceColumn();
    
    $('#btn_tienkhachdua').on('input', function () {
        updatePaymentDetails();
    });
    $('#btn_truvoucher').on('change', function () {
        updatePaymentDetails();
    });
    $('#btn_tienkhachdua').change(function () {
        
        tienthua();
    });
    $('#btn_tienkhachdua').on('focus', handleTienKhachDuaFocus);
    $('body').on('click', '#btn_dathang', function () {
        var today = new Date();

        var date = 'HD' + today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        $('#btn_ma').val(date.toString());
        $('#btn_Status').val(2)
        let tongtiensp = $('#btn_tongtien').val();
        if (tongtiensp == '' || parseFloat(tongtiensp) <= 0) {
            $.notify('Chưa chọn sản phẩm nào ', 'error');
            return;
        }
        let tt = ($('#btn_tienkhachdua').val().replace(/\./g, '').replace(',', '.'));
        let kt = ($('#btn_tienkhachphaitra').val().replace(/\./g, '').replace(',', '.'));
        if ($('#search_khachhang_input').val() == '' || $('#search_khachhang_input').val() == undefined || $('#search_khachhang_input').val() == null) {
            $.notify('Chưa chọn khách hàng!', 'error');
            return;
        }
        if ($('#city').val() == '' || $('#district').val() == '' || $('#ward').val() == '' || $('#street').val() == ''  ) {
            $.notify('Thông tin địa chỉ còn thiếu!', 'error');
            return;
        }
        if ((tt - kt >= 0) || tt == '' || tt == '' || tt == 0) {
            if ($('#btn_phuongthucthanhtoan').val() == 1) {

                Thanhtoan();
                AddLichsuhoadon();
            } 
            if ($('#btn_phuongthucthanhtoan').val() == 2) {

                QRCODE_PAYMENT();
              
            }

        }
        else {
            $.notify('Tiền khách đưa chưa đủ', 'error')
        }
        if ($('#btn_phuongthucthanhtoan').val() == 3) {

            QRCODE_PAYMENT();
           
        }   
        if ($('#btn_phuongthucthanhtoan').val() == 4) {
            Thanhtoan();
           
        }
    })
    async function fetchData() {
        try {
            const response = await fetch('/GetDanhSachUser');
            if (response.ok) {
                const data = await response.json();
                populateTable(data);
            } else {
                console.error('Không thể lấy dữ liệu');
            }
        } catch (error) {
            console.error('Lỗi:', error);
        }
    }

    document.addEventListener('click', function (event) {
        let searchInput = document.getElementById('search_khachhang_input');
        let table = document.getElementById('timkiemkhachhang_table');
        if (!searchInput.contains(event.target) && !table.contains(event.target)) {
            table.classList.add('hidden');
        }
       
    });

    document.getElementById('search_khachhang_input').addEventListener('click', function (event) {
        event.stopPropagation();
    });

    document.getElementById('timkiemkhachhang_table').addEventListener('click', function (event) {
        loadTinhThanh();
        event.stopPropagation();
    });

    window.onload = fetchData;
    $('body').on('click','#btn_chonspchitiet', function () {
        /* $('#btn_MaVoucher').focus();*/
        GetDanhSachVoucher();
        var button = $(this);
        var sku = button.data('sku');
        var tenSp = button.data('tensp');
        var giaBan = parseFloat(button.data('giaban'));
        var maSize = button.data('masize_');
        var maMau = button.data('mamau_');
        var maSP = button.data('masp');
        var maKM = button.data('makm');
        var img = button.data('img');
        var dongia = parseFloat(button.data('dongia'));
        var trangthai = parseInt(button.data('trangthai'));     
        var soLuongKhaDungCell = button.closest('tr').find('.so-luong-kha-dung');
        var slton = parseInt(soLuongKhaDungCell.text().match(/\d+/)[0]) - 1;
        if (slton <= 0) {
            soLuongKhaDungCell.text("Số lượng khả dụng (" + 0 + ")");
        }
        else {
          soLuongKhaDungCell.text("Số lượng khả dụng (" + slton + ")");
        }

        // Cập nhật thuộc tính data-soluongton mới của button
        $(this).data('soluongton', slton);

        var data = {
            SKU: sku,
            MaSp: maSP,
            MaSize: maSize,
            MaMau: maMau,
            MaKhuyenMai: maKM,
            UrlAnhSpct: img,
            DonGia: dongia,
            GiaBan: giaBan,
            SoLuongTon: slton,
            TrangThai: trangthai
        }
        $.ajax({
            url: '/Update_soluongCTsanpham',
            method: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify([data]),
            success: function (re) {
                if (re.status) {
                
                } else {
                    console.error('Cập nhật số lượng sản phẩm thất bại: ' + re.message);
                }
            },
            error: function () {
                console.error('Có lỗi xảy ra khi gửi yêu cầu.');
            }
        });
    })
    $('body').on('click', '#btn_chondiachi', function () {
        $('#pop_diachikhachhang').modal('show')
        $('#foter_diachi').hide();
        $('#btn_ghichu').val('');
    })
    $('body').on('click', '#btn_themnhanhkhachhang', function () {
        $('#pop_themkhachhang').modal('show')
        $('#themsdt_btn').show();
    })
    $('#themsdt_btn').hide();
    $('#foter_diachi').hide();
    $('body').on('click', '#themdiachi_btn', function () {
        $('#pop_diachikhachhang').modal('show');
        $('#foter_diachi').show();
    })
    $('body').on('click', '#nhaplaithongtin_cl', function () {
        $('#pop_diachikhachhang').modal('hide')
        $('#pop_themkhachhang').modal('show')
        $('#foter_diachi').show()
    })
    $('body').on('click', '#themdiachi_btn', function () {
        $('#pop_diachikhachhang').modal('show');
        $('#pop_themkhachhang').modal('hide');
        $('#foter_diachi').show();
    })
    $('body').on('click', '#huy_cl', function () {
        $('#btn_fullname').val('');
        $('#btn_Email').val('');
        $('#btn_Password').val('');
        $('#btn_ConfirmPassword').val('');
        $('#btn_ghichu').val('');
        $('#themsdt_btn').hide();
        $('#btn_fullname').val('');
        $('#btn_SoDienThoai_').val('');
        $('#sdtnhanhang_btn').val('');
        $('#ngnhanhang_btn').val('');
        $('#clear').click();
    })
    $('#btn_MaVoucher').focus(function () {
        GetDanhSachVoucher();
    });
    $('#search_khachhang_input').focus(function () {
        fetchData();
    });
   
    $('body').on('click', '#in_hoadon', function () {
        let ma = $('#btn_ma').val(); 

        // Gửi yêu cầu GET để lấy dữ liệu hóa đơn
        $.get('/InHoaDonBanHang', { ma: ma }, function (re) {
            if (re.status) {
                // Gọi hàm In với dữ liệu nhận được từ server
                In(re.data);
            } else {
                console.log(re.message);
            }
        });
    });
    $('body').on('click', '#khongin_hoadon', function () {
        window.location.reload();
    });
    //$('body').on('click', '#btn_inphieu', function () {
    //    let ma = $('#btn_ma').val(); // Mã hóa đơn giả sử này là đúng

    //    // Gửi yêu cầu GET để lấy dữ liệu hóa đơn
    //    $.get('/InHoaDonBanHang', { ma: ma }, function (re) {
    //        if (re.status) {
    //            // Gọi hàm In với dữ liệu nhận được từ server
    //            In(re.data);
    //        } else {
    //            console.log(re.message);
    //        }
    //    });
    //});

    $('body').on('click', '#luukhachhangMoi_cl', function () {
        if ($('#btn_fullname').val() == '' || $('#btn_fullname').val() == null || $('#btn_fullname').val() == undefined) {
            $.notify('Nhập tên khách hàng ', 'error');
            return;
        }
        if ($('#btn_SoDienThoai_').val() == '' || $('#btn_SoDienThoai_').val() == null || $('#btn_SoDienThoai_').val() == undefined) {
            $.notify('Nhập số điện thoại khách hàng ', 'error');
            return;
        }
        if ($('#btn_SoDienThoai_').val() != '') {
            var phoneInput = $('#btn_SoDienThoai_');
            var phoneNumber = phoneInput.val();

            // Biểu thức chính quy kiểm tra số điện thoại bắt đầu bằng 0 và có độ dài từ 10 đến 11 ký tự
            var phonePattern = /^0\d{9,10}$/;

            // Kiểm tra tính hợp lệ của số điện thoại
            if (!phonePattern.test(phoneNumber)) {
                $.notify('Số điện thoại khách hàng không hợp lệ', 'error');
                return;
            }
           
        }
        if ($('#btn_Email').val() == '' || $('#btn_Email').val() == null || $('#btn_Email').val() == undefined) {
            $.notify('Nhập email', 'error');
            return;
        }
        if ($('#btn_Password').val() == '' || $('#btn_Password').val() == null || $('#btn_Password').val() == undefined) {
            $.notify('Nhập password', 'error');
            return;
        }
        if ($('#btn_ConfirmPassword').val() == '' || $('#btn_ConfirmPassword').val() == null || $('#btn_ConfirmPassword').val() == undefined) {
            $.notify('Nhập lại password', 'error');
            return;
        }
        if ($('#btn_ConfirmPassword').val() != $('#btn_Password').val()) {
            $.notify('Nhập lại password không đúng', 'error');
            return;
        }
        if ($('#sdtnhanhang_btn').val() == '' || $('#sdtnhanhang_btn').val() == null || $('#sdtnhanhang_btn').val() == undefined) {
            $.notify('Nhập số điện thoại nhận hàng ', 'error');
            return;
        }
        if ($('#sdtnhanhang_btn').val() != '' ) {
            var phoneInput = $('#sdtnhanhang_btn');
            var phoneNumber = phoneInput.val();

            // Biểu thức chính quy kiểm tra số điện thoại bắt đầu bằng 0 và có độ dài từ 10 đến 11 ký tự
            var phonePattern = /^0\d{9,10}$/;

            // Kiểm tra tính hợp lệ của số điện thoại
            if (!phonePattern.test(phoneNumber)) {
                $.notify('Số điện thoại nhận hàng không hợp lệ', 'error');
                return;
            }
        }
        if ($('#ngnhanhang_btn').val() == '' || $('#ngnhanhang_btn').val() == null || $('#ngnhanhang_btn').val() == undefined) {
            $.notify('Nhập tên nhận hàng ', 'error');
            return;
        }
        if ($('#city').val() == '' || $('#city').val() == null || $('#city').val() == undefined) {
            $.notify('Nhập tỉnh thành', 'error');
            return;
        }
        if ($('#district').val() == '' || $('#district').val() == null || $('#district').val() == undefined) {
            $.notify('Nhập quận huyện', 'error');
            return;
        }
        if ($('#ward').val() == '' || $('#ward').val() == null || $('#ward').val() == undefined) {
            $.notify('Nhập xã phường', 'error');
            return;
        }
        if ($('#street').val() == '' || $('#street').val() == null || $('#street').val() == undefined) {
            $.notify('Nhập địa chỉ cụ thể','error');
            return;
        }
        themnhanhkhachhang();
    })
});
// thanh toán chuyển khoản 
let paymentSuccessful = false;
function QRCODE_PAYMENT() {
    $('#pop_QR').modal('show');
    let countdownTime = 300;
    const countdownElement = document.getElementById("countdown");
    const paymentCheckInterval = setInterval(() => {


        checkpaid(sotienck, thongtinhoadon, paymentCheckInterval);


    }, 3000);
    // Cập nhật đồng hồ đếm ngược mỗi giây
    const countdownInterval = setInterval(function () {
        if (paymentSuccessful) {
            clearInterval(countdownInterval); // Dừng đồng hồ đếm ngược nếu đã thanh toán
            return;
        }
        countdownElement.textContent = 'Giao dịch sẽ kết thúc sau: ' + countdownTime;
        countdownTime--;
        // Khi đồng hồ đếm ngược về 0, đóng modal và dừng đồng hồ đếm ngược
        if (countdownTime < 0) {
            clearInterval(countdownInterval);
            $.notify('Giao dịch không thành công, quá thời gian chờ', 'error');
            $("#pop_QR").modal('hide');
            clearInterval(paymentCheckInterval);
        }
    }, 1000); // Cập nhật mỗi giây
    $('#btn_sotienck').text(($('#btn_tienkhachphaitra').val() != '' ? $('#btn_tienkhachphaitra').val() : 0) + ' VNĐ');

    //let sotienck = 5000
    //thongtinhoadon = 'YUKAOZFVFD';
    
    thongtinhoadon = $('#btn_maQR').val();
    let sotienck = parseInt($('#btn_tienkhachphaitra').val().replace('.', ''));
    let QR = `https://img.vietqr.io/image/MB-0336262156-qr_only.png?amount=${sotienck}&addInfo=${thongtinhoadon}`

    /*let QR = `https://img.vietqr.io/image/TCB-983333666888-qr_only.png?amount=${sotienck}&addInfo=${thongtinhoadon}`*/
    $('#imgQR').attr('src', QR);

    //setInterval(() => {
    //    if (!shouldStopQRCodePayment) {
    //        checkpaid(sotienck, thongtinhoadon);
    //    }
    //  // checkpaid(sotienck, thongtinhoadon);
    //}, 1000);

    async function checkpaid(price, content, paymentCheckInterval) {
        try {
            const respon = await fetch("https://script.google.com/macros/s/AKfycbxwZpNWFZTZmzDCaWaxfbHbUX5hHKImRf1bCtbIU8dQkHKmDNhagjFAxkR9TDLGMoMYKQ/exec");
            const datas = await respon.json();

            if (datas.error === false) {
                const lastPaid = datas.data[0];
                let pr = lastPaid['Giá trị'];
                let ct = lastPaid['Mô tả'];
                if (pr >= price && ct.includes(content)) {
                    //   shouldStopQRCodePayment = true;
                    price = 0;
                    content = '';
                    clearInterval(paymentCheckInterval);
                    Thanhtoan();
                    AddLichsuhoadon();
                    $("#pop_QR").modal('hide');
                    return;
                }

                else {
                    console.log('giao dịch không thành công');
                }
            } else {
                console.error("Error in the response:", datas.error);
            }
        } catch (e) {

        }
    }
}
function tienthua() {
    var tienkhachdua = $('#btn_tienkhachdua').val();
    var tienkhachphaitra = $('#btn_tienkhachphaitra').val()
    
    var value = formatMoney(parseFloat(tienkhachdua))
    var value2 = parseFloat(tienkhachphaitra.replace(/\./g, '').replace(',', '.'));

    $('#btn_tienkhachdua').val(value);
    if ($('#btn_phuongthucthanhtoan').val() == '1') {

        let tienthua = tienkhachdua - value2;
        if (tienkhachdua <= 0 || tienkhachdua < value2) {
            $('#btn_tienthua').val(0);
            $('#btn_tienthua_error').show();
        } else {
            $('#btn_tienthua').val(formatMoney(tienthua));
            $('#btn_tienthua_error').hide();
        }
    }
     if ($('#btn_phuongthucthanhtoan').val() == '3') {
         let tienchuyenkhoan = value2 - tienkhachdua;     
         let tck = formatMoney(tienchuyenkhoan);
         $('#btn_tienkhachphaitra').val(tck)
    } 
   
}

function handleTienKhachDuaFocus() {
    var input = $('#btn_tienkhachdua');
    var input2 = $('#btn_tienthua');
    input.val(0); // Reset giá trị của input
    input2.val(0);
}

// kiểm tra số điện thoại
function validatePhoneNumber() {
    var phoneInput = $('#btn_SoDienThoai');
    var phoneError = $('#phoneError');
    var phoneNumber = phoneInput.val();

    // Biểu thức chính quy kiểm tra số điện thoại bắt đầu bằng 0 và có độ dài từ 10 đến 11 ký tự
    var phonePattern = /^0\d{9,10}$/;

    // Kiểm tra tính hợp lệ của số điện thoại
    if (!phonePattern.test(phoneNumber)) {
        phoneError.show();
    } else {
        phoneError.hide();
    }
}

// format money 
//function formatMoney(amount) {
//    return amount.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, '.').replace('.', ',').replace(/(\d+),(\d+)$/, '$1,$2').replace(/,/g, '.').replace(/(\d+)\.(\d+)$/, '$1,$2')
//}
function formatMoney(amount) {
    if (!isNaN(amount) && amount !== null && amount !== '') {
        return new Intl.NumberFormat( { style: 'currency', currency: 'VND' }).format(amount);
    } else {
        return amount;
    }
}
function filterFunction() {
    var searchInput = document.getElementById("search-input").value.toUpperCase().trim().replace(/\s+/g, ' ');
    var selectedColor = document.getElementById("select-color").value;
    var selectedSize = document.getElementById("select-size").value;

    var table = document.getElementById("sanpham_table");
    var rows = table.getElementsByTagName("tbody")[0].getElementsByTagName("tr");

    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var color = row.querySelector('[data-mamau]').getAttribute('data-mamau').trim();
        var size = row.querySelector('[data-masize]').getAttribute('data-masize').trim();
        var tenSp = row.querySelector('[data-tensp]').getAttribute('data-tensp').toUpperCase().trim();
        if ((selectedColor === "Chọn màu" || color === selectedColor) &&
            (selectedSize === "Chọn size" || size === selectedSize) &&
            (tenSp.includes(searchInput))) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    }
}


// Function to add event listeners to select buttons
// Function to add event listeners to select buttons
function addSelectButtonEventListeners() {
    var selectButtons = document.querySelectorAll('#sanpham_table .btn_chonsp');

    selectButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            var sku = button.getAttribute('data-sku');
            var tenSp = button.getAttribute('data-tensp');
            var giaBan = parseFloat(button.getAttribute('data-giaban'));
            var maSize = button.getAttribute('data-masize');
            var maSize_ = button.getAttribute('data-masize_');
            var maMau_ = button.getAttribute('data-mamau_');
            var maMau = button.getAttribute('data-mamau');
            var slton = parseInt(button.getAttribute('data-soluongton'));
            var maSP = button.getAttribute('data-masp');
            var maKM = button.getAttribute('data-makm');
            var img = button.getAttribute('data-img');
            var dongia = parseFloat(button.getAttribute('data-dongia'));
            var trangthai = parseInt(button.getAttribute('data-trangthai'));
            addToInvoiceDetails(sku, tenSp, giaBan, maSize, maSize_, maMau_, maMau, slton, maSP, maKM, img, dongia, trangthai);
        });
    });
}

function addToInvoiceDetails(sku, tenSp, giaBan, maSize, maSize_, maMau_, maMau, slton, maSP, maKM, img, dongia, trangthai) {
    var table = document.getElementById("chitiethoadon_table");
    if (!table) return; // Kiểm tra table có tồn tại hay không

    var tbody = table.getElementsByTagName("tbody")[0];
    if (!tbody) return; // Kiểm tra tbody có tồn tại hay không

    var rows = tbody.getElementsByTagName("tr");

    // Kiểm tra nếu sản phẩm đã tồn tại trong bảng chi tiết hóa đơn
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        if (row.cells.length > 0 && row.cells[0].innerText === sku) {
            // Tăng số lượng mua
            var soLuongInput = row.cells[4].querySelector('input');

            if (soLuongInput) {
                var soLuongMua = parseInt(soLuongInput.value);
                if (soLuongMua + 1 > slton) {
                    $.notify('Số lượng mua vượt quá số lượng tồn kho!', 'error');
                    return;
                }
                soLuongInput.value = soLuongMua + 1;
                updateTotalPrice(soLuongInput, giaBan, slton);
                updatePaymentDetails();

            }
            return; // Dừng lại sau khi cập nhật
        }
    }

    // Nếu sản phẩm chưa tồn tại, thêm mới vào bảng
    addNewRow(sku, tenSp, giaBan, maSize, maSize_, maMau_, maMau, slton, maSP, maKM, img, dongia, trangthai);
    updatePaymentDetails();
}

// Function to add a new row to invoice details table
function addNewRow(sku, tenSp, giaBan, maSize, maSize_, maMau_, maMau, slton, maSP, maKM, img, dongia, trangthai) {
    var table = document.getElementById("chitiethoadon_table").getElementsByTagName("tbody")[0];
    var newRow = table.insertRow();
    var skucell = newRow.insertCell(0);
    var tenSpCell = newRow.insertCell(1);
    var maSizeCell = newRow.insertCell(2);
    var maMauCell = newRow.insertCell(3);
    var soLuongCell = newRow.insertCell(4);
    var giaBanCell = newRow.insertCell(5);
    var actionCell = newRow.insertCell(6);

    var maMau_Cell = newRow.insertCell(7);
    var maSize_Cell = newRow.insertCell(8);
    var dongia_CELL = newRow.insertCell(9);
    var giaban_Cell = newRow.insertCell(10);
    var soluongtonCell = newRow.insertCell(11);
    var MaSPCell = newRow.insertCell(12);
    var MaKMCell = newRow.insertCell(13);
    var IMGCell = newRow.insertCell(14);
    var TTCell = newRow.insertCell(15);

    skucell.innerText = sku;
    skucell.style.display = "none";
    tenSpCell.innerText = tenSp;
    maSizeCell.innerText = maSize;
    maMauCell.innerText = maMau;

    // Tạo container cho input và button
    var quantityContainer = document.createElement('div');
    quantityContainer.classList.add('quantity-input');

    // Tạo nút - (bên trái)
    var minusButton = document.createElement('button');
    minusButton.type = "button";
    minusButton.classList.add("btn", "btn-danger", "minus");
    minusButton.innerText = "-";
    minusButton.addEventListener('click', function () {
        
        var quantityInput = quantityContainer.querySelector('input');
        var currentValue = parseInt(quantityInput.value);
        if (currentValue > 1) {
            quantityInput.value = currentValue - 1;
            var button = $(this);
            var row2 = $("#sanpham_table tbody tr").filter(function () {
                return $(this).find("td:contains('" + sku + "')").length > 0;
            });
            var total;
            if (row2.length > 0) {

                var soLuongKhaDung = row2.find('.so-luong-kha-dung').text();
                total = (parseInt(soLuongKhaDung.replace(/\D/g, '')) + 1);
                row2.find('.so-luong-kha-dung').text("Số lượng khả dụng (" + total + ")");

            } else {
                alert("Không tìm thấy sản phẩm với SKU: " + sku);
            }
            var data = {
                SKU: sku,
                MaSp: maSP,
                MaSize: maSize_,
                MaMau: maMau_,
                MaKhuyenMai: maKM,
                UrlAnhSpct: img,
                DonGia: dongia,
                GiaBan: giaBan,
                SoLuongTon: total,
                TrangThai: trangthai
            };
            $.ajax({
                url: '/Update_soluongCTsanpham',
                method: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify([data]),
                success: function (re) {
                    if (re.status) {

                    } else {
                        console.error('Cập nhật số lượng sản phẩm thất bại: ' + re.message);
                    }
                },
                error: function () {
                    console.error('Có lỗi xảy ra khi gửi yêu cầu.');
                }
            });

            updateTotalPrice(quantityInput, giaBan, slton);
            // $('#btn_MaVoucher').focus();
            GetDanhSachVoucher();
        }
    });
    quantityContainer.appendChild(minusButton);

    // Input số lượng
    var quantityInput = document.createElement('input');
    quantityInput.type = "number";
    quantityInput.value = "1";
    quantityInput.min = "1";
    quantityInput.classList.add("form-control");
    quantityInput.readOnly = true; // Chỉ cho phép thay đổi bằng các nút + và -
    quantityContainer.appendChild(quantityInput);

    // Tạo nút + (bên phải)
    var plusButton = document.createElement('button');
    plusButton.type = "button";
    plusButton.classList.add("btn", "btn-success", "plus");
    plusButton.innerText = "+";
    plusButton.addEventListener('click', function () {
        var quantityInput = quantityContainer.querySelector('input');
        var currentValue = parseInt(quantityInput.value);
        if (currentValue < slton) {
            quantityInput.value = currentValue + 1;
            var row2 = $("#sanpham_table tbody tr").filter(function () {
                return $(this).find("td:contains('" + sku + "')").length > 0;
            });
            var total;
            if (row2.length > 0) {

                var soLuongKhaDung = row2.find('.so-luong-kha-dung').text();
                total = (parseInt(soLuongKhaDung.replace(/\D/g, '')) - 1);
                row2.find('.so-luong-kha-dung').text("Số lượng khả dụng (" + total + ")");

            } else {
                alert("Không tìm thấy sản phẩm với SKU: " + sku);
            }
            var data = {
                SKU: sku,
                MaSp: maSP,
                MaSize: maSize_,
                MaMau: maMau_,
                MaKhuyenMai: maKM,
                UrlAnhSpct: img,
                DonGia: dongia,
                GiaBan: giaBan,
                SoLuongTon: total,
                TrangThai: trangthai
            };
            $.ajax({
                url: '/Update_soluongCTsanpham',
                method: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify([data]),
                success: function (re) {
                    if (re.status) {

                    } else {
                        console.error('Cập nhật số lượng sản phẩm thất bại: ' + re.message);
                    }
                },
                error: function () {
                    console.error('Có lỗi xảy ra khi gửi yêu cầu.');
                }
            });
            updateTotalPrice(quantityInput, giaBan, slton);
            // $('#btn_MaVoucher').focus();
            GetDanhSachVoucher();
        } else {
            $.notify('Số lượng mua vượt quá số lượng khả dụng!', 'error');
        }
    });
    quantityContainer.appendChild(plusButton);

    // Thêm container vào ô số lượng
    soLuongCell.appendChild(quantityContainer);

    /* soLuongCell.innerHTML = '<input type="number" value="1" min="1" class="form-control" oninput="updateTotalPrice(this, ' + giaBan + ', ' + slton + ')">';*/

    giaBanCell.innerText = formatMoney(giaBan);
    actionCell.innerHTML = `<button type="button" class="btn btn-danger" onclick="removeRow(this)"><i class="fa-solid fa-trash"></i></button>`;

    maMau_Cell.innerText = maMau_;
    maMau_Cell.style.display = "none";

    maSize_Cell.innerText = maSize_;
    maSize_Cell.style.display = "none";
    dongia_CELL.innerText = dongia;
    dongia_CELL.style.display = "none";
    giaban_Cell.innerText = giaBan;
    giaban_Cell.style.display = "none";
    soluongtonCell.innerText = slton;
    soluongtonCell.style.display = "none";
    MaSPCell.innerText = maSP;
    MaSPCell.style.display = "none";
    MaKMCell.innerText = maKM;
    MaKMCell.style.display = "none";
    IMGCell.innerText = img;
    IMGCell.style.display = "none";
    TTCell.innerText = trangthai;
    TTCell.style.display = "none";



    updatePaymentDetails();
}
// Function to update total price based on quantity change

function updateTotalPrice(input, giaBan, slton) {
    var row = input.closest('tr');
    if (!row) return; // Kiểm tra row có tồn tại hay không
    var soLuongMua = parseInt(input.value);
    if (soLuongMua > slton) {
        alert('Số lượng mua vượt quá số lượng khả dụng.');
        input.value = slton;
        soLuongMua = slton;
    }
    var totalPriceCell = row.cells[5];
    var soLuongInput = row.cells[4].querySelector('input'); // kiểm tra số lượng mua
    if (!totalPriceCell) return; // Kiểm tra totalPriceCell có tồn tại hay không
    totalPriceCell.innerText = formatMoney(parseFloat(input.value) * giaBan);
    updatePaymentDetails();
}

// Function to remove a row from invoice details table
function removeRow(button) {
    var row = button.closest('tr');
    if (row) {
        var sku = row.cells[0].innerText;
        var maSp = row.cells[12].innerText;
        var maSize = row.cells[8].innerText;
        var maMau = row.cells[7].innerText;
        var soluongmua = row.cells[4].querySelector('input').value;

        var giaBan = parseFloat(row.cells[10].innerText);

        var maKM = row.cells[13].innerText;
        var img = row.cells[14].innerText;
        var dongia = parseFloat(row.cells[9].innerText);
        var trangthai = row.cells[15].innerText;
        var total = 0;
        var row2 = $("#sanpham_table tbody tr").filter(function () {
            return $(this).find("td:contains('" + sku + "')").length > 0;
        });

        if (row2.length > 0) {
            // Lấy giá trị của .so-luong-kha-dung trong dòng tìm thấy       
            var soLuongKhaDung = row2.find('.so-luong-kha-dung').text();
            total = parseInt(soLuongKhaDung.replace(/\D/g, '')) + parseInt(soluongmua);
            row2.find('.so-luong-kha-dung').text("Số lượng khả dụng (" + total + ")");
            //soLuongKhaDung.text('Số lượng khả dụng (' + slton + ')');
            //soluongkhadung_ =( parseInt(soLuongKhaDung) - parseInt(soluongmua))
        } else {
            alert("Không tìm thấy sản phẩm với SKU: " + sku);
        }
        // Chuẩn bị dữ liệu để gửi trong yêu cầu AJAX
        var data = {
            SKU: sku,
            MaSp: maSp,
            MaSize: maSize,
            MaMau: maMau,
            MaKhuyenMai: maKM,
            UrlAnhSpct: img,
            DonGia: dongia,
            GiaBan: giaBan,
            SoLuongTon: total,
            TrangThai: trangthai
        };
        $.ajax({
            url: '/Update_soluongCTsanpham',
            method: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify([data]),
            success: function (re) {
                if (re.status) {

                } else {
                    console.error('Cập nhật số lượng sản phẩm thất bại: ' + re.message);
                }
            },
            error: function () {
                console.error('Có lỗi xảy ra khi gửi yêu cầu.');
            }
        });
        row.remove();
        updatePaymentDetails();
        // $('#btn_MaVoucher').focus();
        GetDanhSachVoucher();
    }
}
// Function to update payment details (total, discount, customer payment, change)
function updatePaymentDetails() {
    var total = 0;
    $('#chitiethoadon_table tbody tr').each(function () {
        // var soLuong = parseFloat($(this).find('input[type="number"]').val());
        //var giaBan = parseFloat($(this).find('td:eq(5)').text().replace(/\./g, '').replace(',', '.')); 
        var giaBan = parseFloat($(this).find('td:eq(5)').text().replace(/\./g, '').replace(',', '.'));

        if (!isNaN(giaBan)) {
            total += giaBan;
        
        }
    });

    var ship = ($('#btn_PhiShip').val() === '') ? 0 : parseFloat($('#btn_PhiShip').val());
    var selectedOption = $('#btn_MaVoucher').find(':selected');
   var giaTriGiam = selectedOption.val() !== '' ? selectedOption.data('giatrigiam') : 0;
    /* var tienkhachdua = ($('#btn_tienkhachdua').val() == '') ? 0 : parseFloat($('#btn_tienkhachdua').val());*/
    var tongtienkhachphaitra = total + ship - giaTriGiam;
    var tongtien = total;
    if (tongtienkhachphaitra <= 0) {
        $('#btn_tienkhachphaitra').val(0);
    }
    else {
        $('#btn_tienkhachphaitra').val(formatMoney(tongtienkhachphaitra));
    }
    $('#btn_tongtien').val(formatMoney(tongtien));

}

  
function getdataHoaDonChiTiet() {
    
    var hoaDonChiTiets = [];
    let mahd = $('#btn_ma').val();
   
    $('#chitiethoadon_table tbody tr').each(function () {
        // Kiểm tra xem dòng này có cells không
        if ($(this).find('td').length > 0) {
            var sku = $(this).find('td:eq(0)').text().trim();
            var tenSp = $(this).find('td:eq(1)').text().trim();
            var maSize = $(this).find('td:eq(2)').text().trim();
            var maMau = $(this).find('td:eq(3)').text().trim();
            var soLuongMua = parseInt($(this).find('td:eq(4) input').val().trim());
            var donGia = parseFloat($(this).find('td:eq(5)').text().trim().replace(/\./g, ''));
            var hoaDonChiTiet = {
                SKU: sku,
                TenSp: tenSp,
                MaHoaDon: mahd,
                DonGia: donGia,
                SoLuongMua: soLuongMua,
                TrangThai: 1
            };

            hoaDonChiTiets.push(hoaDonChiTiet);
        }
    });

    return hoaDonChiTiets;
}
function getdataHoaDon() {
    var tenkhachhang;
    const id = document.getElementById('search_khachhang_input');

  
    const dataIdKh = id.getAttribute('data-idkh');
    if ($('#search_khachhang_input').val() == '') {
        tenkhachhang = 'Khách lẻ';
    }
    if ($('#search_khachhang_input').val() != '') {
        tenkhachhang = dataIdKh;    
    }
    var tongtienhoadon = $('#btn_tienkhachphaitra').val().replace(/\./g, '').split(',')[0]; 
    
    return {
        MaHoaDon: $('#btn_ma').val(),
        UserID: $('#btn_UserID').val(),
        MaVoucher: $('#btn_MaVoucher').val(), 
        NgayTaoDon: $('#btn_NgayTaoDon').val(),
        NgayThayDoi: $('#btn_Update').val(),
        TenKhachHang: tenkhachhang,
        SoDienThoai: $('#sdtnhanhang_btn').val(), // số diện thoại nhận hàng 
        PhiShip: $('#btn_PhiShip').val() != '' ? $('#btn_PhiShip').val() : 0,
        TongGiaTriHangHoa: tongtienhoadon,
        PhuongThucThanhToan: $('#btn_phuongthucthanhtoan').val(),
        TrangThai: $('#btn_Status').val(),
        PhanLoai: $('#btn_phanloai').val(),
        TinhThanh: $('#city').val(),
        QuanHuyen: $('#district').val(), 
        XaPhuong: $('#ward').val(),
        Cuthe: $('#street').val(),
        Ghichu: $('#btn_ghichu').val(),
    }                                           
}

function Thanhtoan() {
    var hoaDonChiTiets = getdataHoaDonChiTiet();
    var hoaD = getdataHoaDon();
    let tt = $('#btn_tongtien').val();
    if (tt == '' || parseFloat(tt) <= 0) {
        $.notify('Chưa chọn sản phẩm nào ', 'error');
        return;
    }
    $.post('/Add-hoadon', { hd: hoaD }, function (re) {
        if (re.status) {
            $.ajax({
                url: '/Add-hoadonct',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(hoaDonChiTiets),
                success: function (re) {
                    if (re.status) {      
                        if ($('#btn_Status').val() == 5) {
                            AddThanhTOanHoaDon();
                            //  localStorage.setItem('notification', JSON.stringify({ message: 'Thanh toán thành công', type: 'success' }));
                            //  window.location.reload();$('#inhoadon_modal').modal('show');
                            $.notify('Thanh toán thành công', 'success');
                            $('#inhoadon_modal').modal('show');
                        }
                        if ($('#btn_Status').val() == 2) {
                            AddThanhTOanHoaDon();
                          //  localStorage.setItem('notification', JSON.stringify({ message: 'Tạo đơn thành công', type: 'success' }));
                            //  window.location.reload();
                            $.notify('Thanh toán thành công', 'success');
                            $('#inhoadon_modal').modal('show');
                         
                        }   
                        if ($('#btn_Status').val() == 4) {
                          //  localStorage.setItem('notification', JSON.stringify({ message: 'Tạo đơn thành công', type: 'success' }));
                            AddThanhTOanHoaDon();
                            $.notify('Thanh toán thành công', 'success');
                            $('#inhoadon_modal').modal('show');
                        }
                        
                    } else {
                        $.notify(re.message, 'error');
                    }
                },
                error: function (xhr, status, error) {
                    $.notify('Có lỗi xảy ra: ' + error, 'error');
                }
            });
        }
        else {
            $.notify(re.message, 'error');
        }
    }).fail(function (xhr, status, error) {
        // Xử lý lỗi khi yêu cầu /Add-hoadon thất bại
        alert('Có lỗi xảy ra khi thêm hóa đơn: ' + error);
    });
}
function AddThanhTOanHoaDon() {
    let stt;
    if ($('#btn_phuongthucthanhtoan').val() == 4) {
        stt = 0
    }
    else {
        stt = 1
    }
    var data = {
        MaHoaDon: $('#btn_ma').val(),
        MaPhuongThuc: $('#btn_phuongthucthanhtoan').val(),
        NgayTao: $('#btn_NgayTaoDon').val(),
        NgayThayDoi: $('#btn_NgayTaoDon').val(),
        TrangThai:stt,
    };
   
    $.post('/Add-ThanhToanhoadon', { tt: data }, function (re) {
        if (re.status) {
            
        }
        else {
            console.log('Lưu thanh toán hóa đơn thất bại');
        }

    })
}
function AddLichsuhoadon() {   
    var today = new Date();

    var date = 'HD' + today.getDate() + (today.getMonth() + 1) + today.getFullYear() + today.getHours() + today.getMinutes() + today.getSeconds();
    var tongtienhoadon = $('#btn_tienkhachphaitra').val().replace(/\./g, '').split(',')[0];  
    var selectedUserName = $("#btn_UserID option:selected").text();
    var data = {
        LichSuHoaDonID: date,
        MaHoaDon: $('#btn_ma').val(),
        UserID: $('#btn_UserName').val(),
        NgayTaoDon: $('#btn_NgayTaoDon').val(),
        NgayThayDoi: $('#btn_Update').val(),
        TongGiaTri: parseFloat(tongtienhoadon),
        HinhThucThanhToan: $('#btn_phuongthucthanhtoan').val(),
        ChiTiet: $('#btn_Status').val(),
        TrangThai: parseInt($('#btn_Status').val()) 
    };

    $.post('/Add-lichsuhoadon', { lshd: data }, function (re) {
        if (re.status) {

        }
        else {
            console.log('Lưu Lịch sử hóa đơn thất bại');
        }

    })
}
function renderCity(data) {
    var citis = document.getElementById("city");
    var districts = document.getElementById("district");
    var wards = document.getElementById("ward");
    for (const x of data) {
        citis.options[citis.options.length] = new Option(x.Name, x.Name);
    }
    citis.onchange = function () {
        districts.length = 1;
        wards.length = 1;
        if (this.value != "") {
            const result = data.filter(n => n.Name === this.value);

            for (const k of result[0].Districts) {
                districts.options[districts.options.length] = new Option(k.Name, k.Name);
            }
        }
    };
    districts.onchange = function () {
        wards.length = 1;
        const dataCity = data.filter((n) => n.Name === citis.value);
        if (this.value != "") {
            const dataWards = dataCity[0].Districts.filter(n => n.Name === this.value)[0].Wards;

            for (const w of dataWards) {
                wards.options[wards.options.length] = new Option(w.Name, w.Name);
            }
        }
    };
}
function GetDanhSachVoucher() {
    let dieukien = $('#btn_tongtien').val();
    let processedValue = dieukien.replace(/\./g, '').split(',')[0];
    let dk = parseFloat(processedValue)
    $.ajax({
        url: '/GetDanhSachVouCher/' + dk,
        type: 'GET',
        success: function (data) {
            let select = $('#btn_MaVoucher');
            if (data && data.length > 0) {
                //if (true) {

                //}
                    console.log(data);
                   
                    select.empty(); // Xóa các tùy chọn hiện tại
                    select.append('<option value="">Chọn Voucher</option>'); // Thêm tùy chọn mặc định

                data.forEach(function (voucher) {
                        if (voucher.kieuGiamGia == 1) {

                            var ptgiam = (voucher.giaTriGiam / 100);
                            var sotiendcgiam = parseFloat(dk) * ptgiam;
                            if (sotiendcgiam >= voucher.giaGiamToiDa) {
                                sotiendcgiam = voucher.giaGiamToiDa;
                            }
                            select.append('<option value="' + voucher.maVoucher + '" data-giatrigiam="' + sotiendcgiam + '"data-giatrigiam="' + voucher.kieuGiamGia +'">' + voucher.ten + '</option>');
                        }
                        else {
                            select.append('<option value="' + voucher.maVoucher + '" data-giatrigiam="' + voucher.giaTriGiam + '"data-giatrigiam="' + voucher.kieuGiamGia + '">' + voucher.ten + '</option>');
                        }
                    });
            } else {
                select.empty(); // Xóa các tùy chọn hiện tại
                select.append('<option value="" selected>Chọn Voucher</option>'); // Thêm tùy chọn mặc định
              
            }
            
        },
        error: function () {
            alert('Đã xảy ra lỗi khi lấy danh sách voucher.');
        }

    }); updatePaymentDetails();
}

function themnhanhkhachhang() {
    var roles = $('#btn_role').val();
    var fullname = $('#btn_fullname').val();
    var sdt = $('#btn_SoDienThoai_').val();
    var sdtnh = $('#sdtnhanhang_btn').val();
    var ngnh = $('#ngnhanhang_btn').val();
    var email = $('#btn_Email').val();
    var password = $('#btn_Password').val();
    var cfpassword = $('#btn_Password').val();
    var tinh = $('#city').val();
    var quan = $('#district').val();
    var xa = $('#ward').val();
    var cuthe = $('#street').val();
    var data = {
        fullName: fullname,
        email: email,
        phoneNumber: sdt,
        password: password,
        confirmPassword: cfpassword
    }
   
    var data2 = {
        Consignee: ngnh,
        PhoneNumber: sdtnh,
        City: tinh,
        District: quan,
        Ward: xa,
        Street: cuthe,
    }
  
    console.log(data)
    $.ajax({
        url: 'https://localhost:7197/api/Register?role=User',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json',
        success: function (response) {
            if (response) {
                $.ajax({
                    url: '/Add_AdressFlash',
                    type: 'POST',
                    data: {
                        sdt: sdt,
                        sdtnhanhang: sdtnh,
                        delivery: data2
                    },
                    success: function (response) {
                        $.notify('Thêm khách hàng thành công', 'success');
                         $('#btn_role').val('');
                         $('#btn_fullname').val('');
                         $('#btn_SoDienThoai_').val('');
                         $('#sdtnhanhang_btn').val('');
                         $('#ngnhanhang_btn').val('');
                         $('#btn_Email').val('');
                         $('#btn_Password').val('');
                        $('#btn_ConfirmPassword').val('');
                         $('#city').val('');
                         $('#district').val('');
                         $('#ward').val('');
                         $('#street').val('');
                         $('#pop_diachikhachhang').modal('hide');
                    },
                    error: function (xhr, status, error) {
                        $.notify(xhr.responseText);

                        return;
                    }
                });
            } else {
                $.notify(re.message, 'error');
            }
        },
        error: function (xhr, status, error) {
       
            $.notify(xhr.responseText);
           
            return;
        }
    });
}

function populateTable(data) {
    const tableBody = document.querySelector('#timkiemkhachhang_table tbody');
    tableBody.innerHTML = ''; // Xóa dữ liệu cũ

    data.forEach(user => {
        const row = document.createElement('tr');
        row.onclick = () => selectValue(row);

        const nameCell = document.createElement('td');
        nameCell.textContent = user.ten;
        row.appendChild(nameCell);

        const phoneCell = document.createElement('td');
        phoneCell.textContent = user.sdt;
        row.appendChild(phoneCell);

        const idCell = document.createElement('td');
        idCell.textContent = user.idUser;
        idCell.classList.add('hidden');
        idCell.textContent = user.idUser;
        row.appendChild(idCell);



        const tinhCell = document.createElement('td');
        tinhCell.textContent = user.tinhthanh;
        tinhCell.classList.add('hidden');
        row.appendChild(tinhCell);

        const quanCell = document.createElement('td');
        quanCell.textContent = user.quanhuyen;
        quanCell.classList.add('hidden');
        row.appendChild(quanCell);

        const xaCell = document.createElement('td');
        xaCell.textContent = user.xaphuong;
        xaCell.classList.add('hidden');
        row.appendChild(xaCell);

        const cutheCell = document.createElement('td');
        cutheCell.textContent = user.cuthe;
        cutheCell.classList.add('hidden');
        row.appendChild(cutheCell);



        const phonenhanhhanngCell = document.createElement('td');
        phonenhanhhanngCell.textContent = user.sdtnhanhhang;
        phonenhanhhanngCell.classList.add('hidden');
        row.appendChild(phonenhanhhanngCell);

        const ngnhanhhanngCell = document.createElement('td');
        ngnhanhhanngCell.textContent = user.ngnhanhang;
        ngnhanhhanngCell.classList.add('hidden');
        row.appendChild(ngnhanhhanngCell);

        tableBody.appendChild(row);
    });
}

function showTable() {
    document.getElementById('timkiemkhachhang_table').classList.remove('hidden');
}

function filterTable() {
    let input = document.getElementById('search_khachhang_input');
    let filter = input.value.toLowerCase().trim().replace(/\s+/g, ' ');
    let table = document.getElementById('timkiemkhachhang_table');
    let tr = table.getElementsByTagName('tr');

    for (let i = 0; i < tr.length; i++) {
        let tdName = tr[i].getElementsByTagName('td')[0];
        let tdPhone = tr[i].getElementsByTagName('td')[1];
        if (tdName || tdPhone) {
            let nameValue = tdName.textContent.trim().replace(/\s+/g, ' ') || tdName.innerText.trim().replace(/\s+/g, ' ');
            let phoneValue = tdPhone.textContent || tdPhone.innerText;
            if (nameValue.toLowerCase().indexOf(filter) > -1 || phoneValue.toLowerCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

function selectValue(row) {
    let input = document.getElementById('search_khachhang_input');
    let name = row.getElementsByTagName('td')[0].innerText;
    let phone = row.getElementsByTagName('td')[1].innerText;


    // Lấy các thuộc tính bổ sung từ hàng
    let id = row.getElementsByTagName('td')[2].innerText;
    let tinh = row.getElementsByTagName('td')[3].innerText;
    let quanhuyen = row.getElementsByTagName('td')[4].innerText;
    let xaphuong = row.getElementsByTagName('td')[5].innerText;
    let cuthe = row.getElementsByTagName('td')[6].innerText;
    let sdtnhanhhang = row.getElementsByTagName('td')[7].innerText;
    let ngnhanhang = row.getElementsByTagName('td')[8].innerText;

    // Đặt giá trị ô nhập và thuộc tính
    input.value = `${name} - ${phone}`;
    input.setAttribute('data-idKH', id);
    input.setAttribute('data-tinh', tinh);
    input.setAttribute('data-quanhuyen', quanhuyen);
    input.setAttribute('data-xaphuong', xaphuong);
    input.setAttribute('data-cuthe', cuthe);
    input.setAttribute('data-sdtnhanhhang', sdtnhanhhang);
    input.setAttribute('data-ngnhanhang', ngnhanhang);
    if ($('#btn_giaohang').is(':checked') == false) {
        //document.getElementById('city').innerHTML = '<option value="' + tinh + '">' + tinh + '</option>';
        //document.getElementById('district').innerHTML = '<option value="' + quanhuyen + '">' + quanhuyen + '</option>';
        //document.getElementById('ward').innerHTML = '<option value="' + xaphuong + '">' + xaphuong + '</option>';
        //document.getElementById('street').value = cuthe;
        document.getElementById('btn_SoDienThoai').value = phone;
        $('#btn_PhiShip').val(0);

        $('#btn_fullname').val('');
        //   $('#btn_SoDienThoai').val('');
        $('#sdtnhanhang_btn').val('');
        $('#ngnhanhang_btn').val('');
        $('#btn_Email').val('');
        $('#btn_Password').val('');
        $('#btn_Password').val('');
    } else {

        document.getElementById('city').innerHTML = '<option value="' + tinh + '">' + tinh + '</option>';
        document.getElementById('district').innerHTML = '<option value="' + quanhuyen + '">' + quanhuyen + '</option>';
        document.getElementById('ward').innerHTML = '<option value="' + xaphuong + '">' + xaphuong + '</option>';
        document.getElementById('street').value = cuthe;
        document.getElementById('btn_SoDienThoai').value = phone;
        document.getElementById('sdtnhanhang_btn').value = sdtnhanhhang;
        document.getElementById('ngnhanhang_btn').value = ngnhanhang;
        //  loadTinhThanh();
    }
    // Ẩn bảng sau khi chọn hàng
    document.getElementById('timkiemkhachhang_table').classList.add('hidden');

}
function In(data) {

    var selectedOption = $('#btn_MaVoucher').find(':selected');
    // Lấy dữ liệu từ các ô nhập liệu
    var mahoadon = $('#btn_ma').val();
    var tongtien = $('#btn_tongtien').val();
    var ngaytao = $('#btn_NgayTaoDon').val();
    var voucher = selectedOption.text() != 'Chọn Voucher' ? selectedOption.text() : '- 0 VNĐ';

    var phiship = $('#btn_PhiShip_fake').val() != '' ? $('#btn_PhiShip_fake').val() : 0;
    var tienkhachphaitra = $('#btn_tienkhachphaitra').val();
    var khachdua = $('#btn_tienkhachdua').val() != '' ? $('#btn_tienkhachdua').val() : 0;
    var tienthua = $('#btn_tienthua').val() != '' ? $('#btn_tienthua').val() : 0;
    var nv = $('#btn_UserName').val();
    var pt_thanhtoan = pt;
    var trangthai = thanhtoan;
    var tenkhachhang;
    var sdt;
    var tinhthanh;
    var quanhuyen;
    var xaphuong;
    var cuthe;
    var diachi;

    const id = document.getElementById('search_khachhang_input');
    if ($('#search_khachhang_input').val() == '') {
        tenkhachhang = '';
        sdt = '';
        tinhthanh= '';
        quanhuyen= '';
        xaphuong = '';
        cuthe = '';
        diachi = '';
    }
    if ($('#search_khachhang_input').val() != '') {
       
        var parts = id.value.split('-');
        sdt = parts[1].trim();
        tenkhachhang = parts[0].trim();
        tinhthanh = $('#search_khachhang_input').attr('data-tinh');
        quanhuyen = $('#search_khachhang_input').attr('data-quanhuyen');
        xaphuong = $('#search_khachhang_input').attr('data-xaphuong');
        cuthe = $('#search_khachhang_input').attr('data-cuthe');
        diachi = tinhthanh + '-' + quanhuyen + '-' + xaphuong + '-' + cuthe;
    }
    
    var printContent = `
        <div style="text-align: center;">
             <img id="shopLogo" src="https://localhost:7060/img/logo/image.png" alt="Shop Logo" style="max-width: 200px;height: 35px; ">
             <p> Số 1, Cầu Noi,P.Cổ Nhuế 2, Q.Bắc Từ Liêm</p>
             <p>Liên hệ : 0461728398</p>
              <hr />
              <h4>HÓA ĐƠN BÁN HÀNG</h4>
             <p>Số : ${mahoadon}</p>
        </div>
        <div class="col-md-12" style="text-align:left;margin-left:10px">
        <p>Khách hàng :  ${tenkhachhang}</p>
        <p>ĐT :  ${sdt}</p>
        <p>Địa Chỉ :${diachi} </p>
        </div>
        <table style="width: 400px; border-collapse: collapse;">
            <thead>
                    <tr>
                        <th style="text-align : center;">Tên Sản Phẩm</th>
                        <th style="text-align : center;">Size</th>
                        <th style="text-align : center;">Màu</th>
                        <th style="text-align : center;">Số Lượng</th>
                        <th style="text-align : center;">Đơn Giá</th>
                    </tr>
                </thead>
                <tbody>
            `;
    data.forEach(item => {
        printContent += `
            <tr>
                <td style="text-align : center;">${item.tenSp_}</td>
                <td style="text-align : center;">${item.tenSize_}</td>
                <td style="text-align : center;">${item.tenMau_}</td>
                <td style="text-align : center;">${item.soLuongMua_}</td>
                <td style="text-align : center;">${formatMoney(item.donGia_)}</td>
            </tr>
        `;
    });

    printContent += `
            </tbody>
        </table>
        <div class="shop-info"  style="text-align:left">
             <table style="width: 400px; border-collapse: collapse;">
                <tr><th style="padding-left:10px;width:150px">Tổng tiền hàng</th><td>: ${tongtien} VNĐ</td></tr>
                <tr><th style="padding-left:10px;width:150px">Voucher</th><td>: ${voucher}</td></tr>
                <tr><th style="padding-left:10px;width:150px">Phí ship</th><td>: ${phiship} VNĐ</td></tr>
                <tr><th style="padding-left:10px;width:150px">Tổng tiền</th><td>: ${tienkhachphaitra} VNĐ</td></tr>
                <tr><th style="padding-left:10px;width:150px">Khách đưa</th><td>: ${khachdua} VNĐ</td></tr>
                <tr><th style="padding-left:10px;width:150px">Tiền thừa</th><td>: ${tienthua} VNĐ</td></tr>
             </table>
         </div>
    `;

    $('#printableTable').html(printContent);
    $('#shopLogo').on('load', function () {
       
        window.onafterprint = function () {
            location.reload();
        };
        window.print();
    });

}