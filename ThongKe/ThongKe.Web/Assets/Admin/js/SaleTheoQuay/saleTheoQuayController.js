
$.validator.addMethod("dateFormat",
    function (value, element) {
        var check = false;
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        if (re.test(value)) {
            var adata = value.split('/');
            var dd = parseInt(adata[0], 10);
            var mm = parseInt(adata[1], 10);
            var yyyy = parseInt(adata[2], 10);
            var xdata = new Date(yyyy, mm - 1, dd);
            if ((xdata.getFullYear() === yyyy) && (xdata.getMonth() === mm - 1) && (xdata.getDate() === dd)) {
                check = true;
            }
            else {
                check = false;
            }
        } else {
            check = false;
        }
        return this.optional(element) || check;
    },
    "Chưa đúng định dạng dd/mm/yyyy.");

//$.validator.addMethod(
//    "dateFormat",
//    function (value, element) {
//        var check = false;
//        var re = /^\d{1,2}\-\d{1,2}\-\d{4}$/;
//        if (re.test(value)) {
//            var adata = value.split('-');
//            var dd = parseInt(adata[0], 10);
//            var mm = parseInt(adata[1], 10);
//            var yyyy = parseInt(adata[2], 10);
//            var xdata = new Date(yyyy, mm - 1, dd);
//            if ((xdata.getFullYear() === yyyy) && (xdata.getMonth() === mm - 1) && (xdata.getDate() === dd)) {
//                check = true;
//            }
//            else {
//                check = false;
//            }
//        } else {
//            c   heck = false;
//        }
//        return this.optional(element) || check;
//    },
//    "Wrong date format"
//);

var saleTheoQuayController = {
    init: function () {
        // saleTheoQuayController.LoadData();
        saleTheoQuayController.registerEvent();
    },

    registerEvent: function () {

        //$('.modal-dialog').draggable();

        $('#frmSearch').validate({
            rules: {
                tungay: {
                    required: true,
                    dateFormat: true
                    //date: true
                },
                denngay: {
                    required: true,
                    dateFormat: true
                    //date: true
                }
            },
            messages: {
                tungay: {
                    required: "Trường này không được để trống."
                    //date: "Chưa đúng định dạng."
                },
                denngay: {
                    required: "Trường này không được để trống."
                    //number: "password phải là số",
                    //date: "Chưa đúng định dạng."
                }
            }
        });


        $('#btnSearch').off('click').on('click', function () {
            $('#frmSearch').submit();
        });

        $('#btnReset').off('click').on('click', function () {
            saleTheoQuayController.resetForm();
        });

        $("#txtTuNgay, #txtDenNgay").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: "dd/mm/yy"

        });
    },
    resetForm: function () {
        $('#txtTuNgay').val('');
        $('#txtDenNgay').val('');
    }

}
saleTheoQuayController.init();