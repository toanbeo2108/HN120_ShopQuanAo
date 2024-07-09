//import { Console } from "console";

$(document).ready(function () {
    var notification = localStorage.getItem('notification');
    if (notification) {
        notification = JSON.parse(notification);
        $.notify(notification.message, notification.type);
        localStorage.removeItem('notification');
    } 
    $('#thanhtoantaiquay').show();
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
        if (isChecked ) {
            $('#btn_thongtingiaohang').show();
            $('#dathangtaiquay').show();
            $('#thanhtoantaiquay').hide();
            
        } else {
            $('#btn_thongtingiaohang').hide();
            $('#dathangtaiquay').hide();
            $('#thanhtoantaiquay').show();
            //$('#btn_PhiShip').val(0);
            //updatePaymentDetails();
            //$('#btn_PhiShip_fake').val('');
            $('#btn_TenKhachHang').val('');
            $('#btn_SoDienThoai').val('');
            $('#clear').click()
        }
    })
    var token = '8fbfedf6-b458-11ee-b6f7-7a81157ff3b1';
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
    $('body').on('click', '#btn_thanhtoanhoadon', function () {
        if ($('#btn_phuongthucthanhtoan').val() == '4') {

            $.notify('Phương thức thanh toán chỉ áp dụng cho đặt hàng tại quầy', 'error');
            return;
        }
        if ($('#btn_TenKhachHang').val() != '' && $('#btn_SoDienThoai').val() == '') {
            $.notify('Vui lòng nhập số điện thoại', 'error');
            return;
        }
        if ($('#btn_TenKhachHang').val() != '') {
            var phoneInput = $('#btn_SoDienThoai');
            var phoneError = $('#phoneError');
            var phoneNumber = phoneInput.val();

            // Biểu thức chính quy kiểm tra số điện thoại bắt đầu bằng 0 và có độ dài từ 10 đến 11 ký tự
            var phonePattern = /^0\d{9,10}$/;

            // Kiểm tra tính hợp lệ của số điện thoại
            if (!phonePattern.test(phoneNumber)) {
                $.notify('Số điện thoại không hợp lệ', 'error');
                return;
            }
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
        loadTinhThanh();
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
        $('#btn_Status').val(1)

        let tt = ($('#btn_tienkhachdua').val().replace(/\./g, '').replace(',', '.'));
        let kt = ($('#btn_tienkhachphaitra').val().replace(/\./g, '').replace(',', '.'));
        if ($('#btn_TenKhachHang').val() != '' && $('#btn_SoDienThoai').val() == '') {
            $.notify('Vui lòng nhập số điện thoại', 'error');
            return;
        }
        if ($('#btn_TenKhachHang').val() != '') {
            var phoneInput = $('#btn_SoDienThoai');
            var phoneError = $('#phoneError');
            var phoneNumber = phoneInput.val();

            // Biểu thức chính quy kiểm tra số điện thoại bắt đầu bằng 0 và có độ dài từ 10 đến 11 ký tự
            var phonePattern = /^0\d{9,10}$/;

            // Kiểm tra tính hợp lệ của số điện thoại
            if (!phonePattern.test(phoneNumber)) {
                $.notify('Số điện thoại không hợp lệ', 'error');
                return;
            }
        }
        if ($('#city').val() == '' || $('#district').val() == '' || $('#ward').val() == '' || $('#street').val() == '' || $('#btn_SoDienThoai') == '' ) {
            $.notify('Vui lòng điền đầy đủ địa chỉ, và số điện thoại!', 'error');
                return;
        }
        if ($('#btn_SoDienThoai') != '') {
            var phoneInput = $('#btn_SoDienThoai');
            var phoneError = $('#phoneError');
            var phoneNumber = phoneInput.val();

            // Biểu thức chính quy kiểm tra số điện thoại bắt đầu bằng 0 và có độ dài từ 10 đến 11 ký tự
            var phonePattern = /^0\d{9,10}$/;

            // Kiểm tra tính hợp lệ của số điện thoại
            if (!phonePattern.test(phoneNumber)) {
                $.notify('Số điện thoại không hợp lệ', 'error');
                return;
            }
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
        if ($('#btn_phuongthucthanhtoan').val() == 3) {

            QRCODE_PAYMENT();
           
        }   
        if ($('#btn_phuongthucthanhtoan').val() == 4) {
            Thanhtoan();
            AddLichsuhoadon();
        }
    })

    $('#btn_TenKhachHang').change(function () {
        var selectedOption = this.options[this.selectedIndex];
        var tinhThanh = selectedOption.getAttribute('data-tinh');
        var quanHuyen = selectedOption.getAttribute('data-quanhuyen');
        var xaPhuong = selectedOption.getAttribute('data-xaphuong');
        var cuThe = selectedOption.getAttribute('data-cuthe');
        var sodt = selectedOption.getAttribute('data-sodienthoai');
        if ($('#btn_btn_TenKhachHang').val != '') {
            document.getElementById('city').innerHTML = '<option value="' + tinhThanh + '">' + tinhThanh + '</option>';
            document.getElementById('district').innerHTML = '<option value="' + quanHuyen + '">' + quanHuyen + '</option>';
            document.getElementById('ward').innerHTML = '<option value="' + xaPhuong + '">' + xaPhuong + '</option>';
            document.getElementById('street').value = cuThe; document.getElementById('btn_SoDienThoai').value = sodt;
        }
        
        loadTinhThanh();

    });

    $('body').on('click','#btn_chonspchitiet', function () {
        
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
        soLuongKhaDungCell.text("Số lượng khả dụng (" + slton + ")");

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
});
// thanh toán chuyển khoản 
function QRCODE_PAYMENT() {
    $('#pop_QR').modal('show');
    let countdownTime = 300;
    const countdownElement = document.getElementById("countdown");

    // Cập nhật đồng hồ đếm ngược mỗi giây
    const countdownInterval = setInterval(function () {
        countdownElement.textContent = 'Giao dịch sẽ kết thúc sau: ' + countdownTime;
        countdownTime--;
        // Khi đồng hồ đếm ngược về 0, đóng modal và dừng đồng hồ đếm ngược
        if (countdownTime < 0) {
            clearInterval(countdownInterval);
            localStorage.setItem('notification', JSON.stringify({ message: 'Giao dịch không thành công, quá thời gian chờ', type: 'error' }));
          
            $("#pop_QR").modal('hide');
            window.location.reload();
        }
    }, 1000); // Cập nhật mỗi giây
    $('#btn_sotienck').text(($('#btn_tienkhachphaitra').val() != '' ? $('#btn_tienkhachphaitra').val() : 0) + ' VNĐ');

    thongtinhoadon = $('#btn_maQR').val();
    //string thongtinhoadon = a;
    let sotienck = parseInt($('#btn_tienkhachphaitra').val().replace('.',''));
    let QR = `https://img.vietqr.io/image/MB-0336262156-qr_only.png?amount=${sotienck}&addInfo=${thongtinhoadon}`
    $('#imgQR').attr('src', QR);
    setInterval(() => {

        checkpaid(sotienck, thongtinhoadon);
    }, 1000);
}

async function checkpaid(price , content) {
    try {
        const respon = await fetch("https://script.google.com/macros/s/AKfycbxwZpNWFZTZmzDCaWaxfbHbUX5hHKImRf1bCtbIU8dQkHKmDNhagjFAxkR9TDLGMoMYKQ/exec");
        const datas = await respon.json();

        if (datas.error === false) {
            const lastPaid = datas.data[0];
            let pr = lastPaid['Giá trị'];
            let ct = lastPaid['Mô tả'];           
            if (pr >= price && ct.includes(content)) {                
                Thanhtoan(); 
                AddLichsuhoadon();
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
        } else {
            $('#btn_tienthua').val(formatMoney(tienthua));
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
    input.val(''); // Reset giá trị của input
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
function formatMoney(amount) {
    return amount.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, '.').replace('.', ',').replace(/(\d+),(\d+)$/, '$1,$2').replace(/,/g, '.').replace(/(\d+)\.(\d+)$/, '$1,$2')
}
function filterFunction() {
    var searchInput = document.getElementById("search-input").value.toUpperCase();
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
        if (row.cells.length > 0 && row.cells[0].innerText === sku && row.cells[1].innerText === tenSp && row.cells[2].innerText === maSize && row.cells[3].innerText === maMau) {
            // Tăng số lượng mua
            var soLuongInput = row.cells[4].querySelector('input');
            
            if (soLuongInput) {
                var soLuongMua = parseInt(soLuongInput.value);
                if (soLuongMua + 1 > slton) {
                    alert('Số lượng mua vượt quá số lượng tồn kho.');
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
function addNewRow( sku, tenSp, giaBan, maSize, maSize_, maMau_, maMau, slton, maSP, maKM, img, dongia, trangthai) {
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
        } else {
            alert('Số lượng mua vượt quá số lượng khả dụng.');
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
    var giaTriGiam = selectedOption.val() !== '' ? parseFloat(selectedOption.data('giatrigiam').replace(/\./g, '').replace(',', '.')) : 0;
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
//function setdataHoaDon(data) {
//    if (data == null || data == undefined || data == '') {
//        $('#btn_ma').val('');
//        $('#btn_UserID').val('');
//        $('#btn_MaVoucher').val('');
//        $('#btn_NgayTaoDon').val(moment().format('YYYY-MM-DD HH:mm:ss'));
//        $('#btn_TenKhachHang').val('');
//        $('#btn_SoDienThoai').val('');
//        $('#btn_PhiShip').val('');
//        $('#btn_tongtien').val('');
//        $('#btn_PhuongTTT').val('');
//        $('#btn_Status').val(5);    
//    }
//    else {
//        var nt = moment(data.ngayTaoDon).format('YYYY-MM-DD HH:mm:ss');
//        $('#btn_ma').val(data.maHoaDon);
//        $('#btn_UserID').val(data.userID);
//        $('#btn_MaVoucher').val(data.maVoucher);
//        $('#btn_NgayTaoDon').val(data.ngayTaoDon) != null ? $('#btn_NgayTaoDon').val(nt) : '';
//        $('#btn_TenKhachHang').val(data.tenKhachHang);
//        $('#btn_SoDienThoai').val(data.soDienThoai);
//        $('#btn_PhiShip').val(data.phiShip);
//        $('#btn_tongtien').val(data.tongGiaTriHangHoa);
//        $('#btn_PhuongTTT').val(data.phuongThucThanhToan);
//        $('#btn_Status').val(data.trangThai);
//    }
//}
function getdataHoaDon() {
    var tenkhachhang = $('#btn_TenKhachHang').val()
    if (tenkhachhang == '') {
        tenkhachhang = 'Khách lẻ';
    }
    var tongtienhoadon = $('#btn_tienkhachphaitra').val().replace(/\./g, '').split(',')[0];   
    return {
        MaHoaDon: $('#btn_ma').val(),
        UserID: $('#btn_UserID').val(),
        MaVoucher: $('#btn_MaVoucher').val(), 
        NgayTaoDon: $('#btn_NgayTaoDon').val(),
        TenKhachHang: tenkhachhang,
        SoDienThoai: $('#btn_SoDienThoai').val(),
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
                            localStorage.setItem('notification', JSON.stringify({ message: 'Thanh toán thành công', type: 'success' }));
                            window.location.reload();
                          
                        }
                        if ($('#btn_Status').val() == 1) {
                            AddThanhTOanHoaDon();
                            localStorage.setItem('notification', JSON.stringify({ message: 'Tạo đơn thành công', type: 'success' }));
                            window.location.reload();
                         
                        }   
                        if ($('#btn_Status').val() == 4) {
                            localStorage.setItem('notification', JSON.stringify({ message: 'Tạo đơn thành công', type: 'success' }));
                            AddThanhTOanHoaDon();
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
        NgayThayDoi: $('#btn_NgayTaoDon').val(),
        TongGiaTri: parseFloat(tongtienhoadon),
        HinhThucThanhToan: $('#btn_phuongthucthanhtoan').val(),
        ChiTiet: $('#btn_Status').val(),
        TrangThai: parseInt($('#btn_Status').val()) 
    };

    $.post('/Add-lichsuhoadon', { lshd: data }, function (re) {
        if (re.status) {

        }
        else {
            console.log('Lưu thanh toán hóa đơn thất bại');
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