var Model = kendo.data.Model.define({
    id: "Id",
    fields: {
        "Id": { editable: false, nullable: true },
        "Name": { type: "string" },
        "Phone": { type: "string" },
        "Address": { type: "string" },
        "DefaultImageId": { type: "string"},
        "Note": { type: "string" },
    }
});

var viewModel = kendo.observable({
    editShow: true,
    searchShow: false,
    isEditMode: false,
    imgClick: function ()
    {
        promot.render('請輸入圖片來源:', 'changeText');
    },
    editClick: function (e) {
        this.set('model', e.data);
        this.set('editShow', true);
        this.set('searchShow', false);
        this.set('isEditMode', true);
    }, 
    addClick: function ()
    {
        this.set('editShow', true);
        this.set('searchShow', false);
        this.set('model', new Model());
        this.set('isEditMode', false);
    },
    deleteClick: function (e) {
        var isDelete = confirm('確定刪除該筆店家?');
        if (isDelete) {
            this.storeDataSource.remove(e.data);
            $.ajax({
                url: "/Store/Delete",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: kendo.stringify(e.data)
            }).done(function (response) {
                if (!response.errors) {
                    alert(String.format('{0}刪除成功', response.Name));
                    viewModel.backClick();
                }
                else {
                    alert('刪除失敗');
                }
            });
        }
    },
    backClick: function ()
    {
        this.set('editShow', false);
        this.set('searchShow', true);
    },
    saveClick: function ()
    {
        if (isValid()) {
            if (this.isEditMode) {
                $.ajax({
                    url: "/Store/Update",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    dataType: "json",
                    data: kendo.stringify(viewModel.model)
                }).done(function (response) {
                    if (!response.errors) {
                        viewModel.storeDataSource.read();
                        alert('儲存成功');
                        viewModel.backClick();
                    }
                    else {
                        alert('儲存失敗');
                    }
                });
            }
            else {
                $.ajax({
                    url: "/Store/Create",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    dataType: "json",
                    data: kendo.stringify(viewModel.model)
                }).done(function (response) {
                    if (!response.errors) {
                        viewModel.storeDataSource.read();
                        alert('儲存成功');
                        viewModel.backClick();
                    }
                    else {
                        alert('儲存失敗');
                    }
                });
            }
        }
    },
    storeDataSource: new kendo.data.DataSource({
        batch: false,
        transport: {
            read: {
                url: "/Store/List",
                dataType: "json"
            }
        },
        pageSize: 8,
        page: 1
    }),
    model: new Model(),
});

$(function () {

    kendo.bind($(".body-content"), viewModel);
});


function changeText(val) {
    viewModel.set('model.DefaultImageId', val)
}

function isValid()
{
    var errorMsg = '';
    if (!isRequired(viewModel.model.Name)) {
        errorMsg = errorMsg + '請輸入店名\n';
    }
    if (!isRequired(viewModel.model.Phone)) {
        errorMsg = errorMsg + '請輸入電話\n';
    }
    if (!isRequired(viewModel.model.Address)) {
        errorMsg = errorMsg + '請輸入地址\n';
    }
    if (!isRequired(viewModel.model.DefaultImageId)) {
        errorMsg = errorMsg + '請輸入飲料店圖片來源';
    }
    if (errorMsg.length > 0) {
        alert(errorMsg);
    }

    return errorMsg.length == 0;
}