$(document).ready(function () {
    filterFunction();
    addSelectButtonEventListeners();
    $('#sanpham_table tbody tr').each(function () {
        var giaBanCell = $(this).find('td:eq(7)');
        var giaBan = parseFloat(giaBanCell.text().trim());
        if (!isNaN(giaBan)) {
            giaBanCell.text(formatMoney(giaBan));
        }
    });
    $('#btn_NgayTaoDon').val(moment().format('YYYY-MM-DD'));
    $('body').on('click', '#btn_thanhtoanhoadon', function () {
        $('#btn_NgayTaoDon').val(moment().format('YYYY-MM-DD'));
        var today = new Date();
       
        var date = 'HD' + today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear() + today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
        $('#btn_ma').val(date.toString());
       var hoaDonChiTiets = getdataHoaDonChiTiet();
        $.post('/Add-hoadon', { hd: getdataHoaDon() }, function (re) {          
            if (re.status) {
                $.ajax({
                    url: '/Add-hoadonct',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(hoaDonChiTiets),
                    success: function (re) {
                        if (re.status) {
                            alert(re.message);
                            window.location.reload();
                        } else {
                            alert(re.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert('Có lỗi xảy ra: ' + error);
                    }
                });
            }
            else {
                alert(re.message);
            }
        }).fail(function (xhr, status, error) {
            // Xử lý lỗi khi yêu cầu /Add-hoadon thất bại
            alert('Có lỗi xảy ra khi thêm hóa đơn: ' + error);
        });
      
      
    });
    async function getShippingFee() {
        const token = 'd0d7cce1-3125-11ef-8e53-0a00184fe694'; // Thay thế 'YOUR_TOKEN_HERE' bằng token của bạn
        const qh = parseInt($('#btn_QuanHuyen').val()); // Lấy giá trị từ input Quận/Huyện
        const xp = $('#btn_XaPhuong').val();
        const SshopId = 192652;
        try {
            const response = await $.ajax({
                url: 'https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee',
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Token': token,
                    'ShopId': SshopId
                },
                data: JSON.stringify({
                    "service_type_id": 2,
                    "from_district_id": 1482,
                    "to_district_id": qh,
                    "to_ward_code": xp,
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
                updatePaymentDetails()
            } else {
                console.error('Error:', response.message);
            }
        } catch (error) {
            console.error('Error calling API:', error);
        }
    }
    $('#btn_MaVoucher').change(function () {
        var giaTriGiam = 0;
        var selectedOption = $(this).find(':selected');

        if (selectedOption.val() !== '') {
            giaTriGiam = parseFloat(selectedOption);
        }
       
        updatePaymentDetails(giaTriGiam);
    });
    $('#btn_XaPhuong').change(function () {
        getShippingFee();
     //   
    });
    $('#btn_PhiShip').change(function () {
        updatePaymentDetails();
    });
    $('#btn_MaVoucher').change(updatePaymentDetails);
    $('#btn_PhiShip').on('input', updatePaymentDetails);
    $('#btn_chonsp').click(updatePaymentDetails);

    // Lắng nghe thay đổi số lượng sản phẩm trong bảng chi tiết hóa đơn (giả sử có input số lượng)
    $('#chitiethoadon_table').on('input', 'input[type="number"]', updatePaymentDetails);

});

// format money 
function formatMoney(amount) {
    return amount.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, '.').replace('.', ',').replace(/(\d+),(\d+)$/, '$1,$2').replace(/,/g, '.').replace(/(\d+)\.(\d+)$/, '$1,$2')
}


    // Lấy dữ liệu từ bảng chi tiết hóa đơn
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
            var donGia = parseFloat($(this).find('td:eq(5)').text().trim());

            var hoaDonChiTiet = {
               // MaHoaDonChiTiet: $('#btn_ma').val(),
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
function filterFunction() {
    var searchInput = document.getElementById("search-input").value.toUpperCase();
    var selectedColor = document.getElementById("select-color").value;
    var selectedSize = document.getElementById("select-size").value;

    var table = document.getElementById("sanpham_table");
    var rows = table.getElementsByTagName("tbody")[0].getElementsByTagName("tr");

    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var color = row.cells[6].innerText.trim();
        var size = row.cells[5].innerText.trim();
        var tenSp = row.cells[4].innerText.toUpperCase().trim();

        if ((selectedColor === "Chọn màu" || color === selectedColor) &&
            (selectedSize === "Chọn size" || size === selectedSize) &&
            (tenSp.includes(searchInput))) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    }
}
function addSelectButtonEventListeners() {
    var selectButtons = document.querySelectorAll('#sanpham_table #btn_chonsp');

    selectButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            var sku = button.getAttribute('data-SKU');
            var tenSp = button.getAttribute('data-TenSP');
            var giaBan = parseFloat(button.getAttribute('data-GiaBan'));
            var gia= formatMoney(giaBan)// Chuyển đổi giaBan thành số
            var maSize = button.getAttribute('data-MaSize');
            var maMau = button.getAttribute('data-MaMau');
            var maMau = button.getAttribute('data-MaMau');
            var slton = button.getAttribute('data-SoLuongTon');
            if (slton == 0) {
                /*$('#btn_chonsp').prop('disabled', true);*/
                alert('Sản phẩm đã hết không thể thêm');
            }
            else {

                addToInvoiceDetails(sku, tenSp, giaBan, maSize, maMau);
            }
        });
    });
}

function addToInvoiceDetails(sku, tenSp, giaBan, maSize, maMau) {
    var table = document.getElementById("chitiethoadon_table").getElementsByTagName("tbody")[0];
    var rows = table.getElementsByTagName("tr");

    // Kiểm tra nếu bảng chi tiết hóa đơn chưa có dòng nào
    if (rows.length === 0) {
        addNewRow(table, sku, tenSp, giaBan, maSize, maMau);
        updatePaymentDetails();
        return;
    }

    // Kiểm tra nếu sản phẩm đã tồn tại trong bảng chi tiết hóa đơn
    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        if (row.cells.length > 0 && row.cells[0].innerText === sku && row.cells[1].innerText === tenSp && row.cells[2].innerText === maSize && row.cells[3].innerText === maMau) {
            // Tăng số lượng mua
            var soLuongInput = row.cells[4].getElementsByTagName('input')[0];
            var soLuongMua = parseInt(soLuongInput.value);
            soLuongInput.value = soLuongMua + 1;

            // Cập nhật tổng tiền
            updateTotalPrice(soLuongInput, giaBan);
            updatePaymentDetails();
            return; // Dừng lại sau khi cập nhật
        }
    }

    // Nếu sản phẩm chưa tồn tại, thêm mới vào bảng
    addNewRow(table, sku, tenSp, giaBan, maSize, maMau);
    updatePaymentDetails();
}

function addNewRow(table, sku, tenSp, giaBan, maSize, maMau) {
    var newRow = table.insertRow();
    var skucell = newRow.insertCell(0);
    var tenSpCell = newRow.insertCell(1);
    var maSizeCell = newRow.insertCell(2);
    var maMauCell = newRow.insertCell(3);
    var soLuongCell = newRow.insertCell(4);
    var giaBanCell = newRow.insertCell(5);
    var actionCell = newRow.insertCell(6);

    skucell.innerText = sku;
    tenSpCell.innerText = tenSp;
    maSizeCell.innerText = maSize;
    maMauCell.innerText = maMau;
    soLuongCell.innerHTML = '<input type="number" value="1" min="1"  class="form-control" oninput="updateTotalPrice(this, ' + giaBan + ')">';
    giaBanCell.innerText = giaBan; // Hiển thị giá bán với hai chữ số thập phân
    actionCell.innerHTML = '<button type="button" class="btn btn-danger" onclick="removeRow(this)">Xóa</button>';
}

function updateTotalPrice(input, giaBan) {
    var row = input.closest('tr');
    var soLuong = parseFloat(input.value);
    var totalPriceCell = row.cells[5];
    totalPriceCell.innerText = formatMoney(soLuong * giaBan);; // Hiển thị giá bán với hai chữ số thập phân
    updatePaymentDetails();
}

function removeRow(button) {
    var row = button.closest('tr');
    row.remove();
    updatePaymentDetails();
}

function updatePaymentDetails() {
    var total = 0;
    $('#chitiethoadon_table tbody tr').each(function () {
       // var soLuong = parseFloat($(this).find('input[type="number"]').val());
        //var giaBan = parseFloat($(this).find('td:eq(5)').text().replace(/\./g, '').replace(',', '.')); 
        var giaBan = parseFloat($(this).find('td:eq(5)').text().replace(/\./g, '').replace(',', '.')); 

        if (!isNaN(giaBan) ) {
            total +=  giaBan ;
        }
    });
    var vat = total * 0.1;
    var ship = ($('#btn_PhiShip').val() === '') ? 0 : parseFloat($('#btn_PhiShip').val());
    var selectedOption = $('#btn_MaVoucher').find(':selected');
    var giaTriGiam = selectedOption.val() !== '' ? parseFloat(selectedOption.data('giatrigiam').replace(/\./g, '').replace(',', '.')) : 0;

    var tongtien =  total + ship - giaTriGiam;

    // $('#btn_tongtien').val(tongtien.toFixed(2));
    $('#btn_tongtien').val(formatMoney(tongtien));
    //if ($('#btn_PhiShip').val() == '') {
    //    var tongtien = vat + total;
      
    //    $('#btn_tongtien').val((tongtien.toFixed(2)));
    //}
    //else {
    //    var ship = parseFloat($('#btn_PhiShip').val());
    //    var tongtien = vat + total + ship;
       
    //    $('#btn_tongtien').val((tongtien.toFixed(2)));
    //}
}

function setdataHoaDon(data) {
    if (data == null || data == undefined || data == '') {
        $('#btn_ma').val('');
        $('#btn_UserID').val('');
        $('#btn_MaVoucher').val('');
        $('#btn_NgayTaoDon').val(moment().format('YYYY-MM-DD'));
        $('#btn_TenKhachHang').val('');
        $('#btn_SoDienThoai').val('');
        $('#btn_PhiShip').val('');
        $('#btn_tongtien').val('');
        $('#btn_PhuongTTT').val('');
        $('#btn_Status').val(5);    
    }
    else {
        var nt = moment(data.ngayTaoDon).format('YYYY-MM-DD');
        $('#btn_ma').val(data.maHoaDon);
        $('#btn_UserID').val(data.userID);
        $('#btn_MaVoucher').val(data.maVoucher);
        $('#btn_NgayTaoDon').val(data.ngayTaoDon) != null ? $('#btn_NgayTaoDon').val(nt) : '';
        $('#btn_TenKhachHang').val(data.tenKhachHang);
        $('#btn_SoDienThoai').val(data.soDienThoai);
        $('#btn_PhiShip').val(data.phiShip);
        $('#btn_tongtien').val(data.tongGiaTriHangHoa);
        $('#btn_PhuongTTT').val(data.phuongThucThanhToan);
        $('#btn_Status').val(data.trangThai);
    }
}
function getdataHoaDon() {
    
    return {
        MaHoaDon: $('#btn_ma').val(),
        UserID: $('#btn_UserID').val(),
        MaVoucher: $('#btn_MaVoucher').val(),
        NgayTaoDon: $('#btn_NgayTaoDon').val(),
        TenKhachHang: $('#btn_TenKhachHang').val(),
        SoDienThoai: $('#btn_SoDienThoai').val(),
        PhiShip: $('#btn_PhiShip').val(),
        TongGiaTriHangHoa: $('#btn_tongtien').val(),
        PhuongThucThanhToan: $('#btn_PhuongTTT').val(),
        TrangThai: $('#btn_Status').val(),
    }                                           
}

