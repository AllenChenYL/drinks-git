var detail ={
    OrderId: 0,
    CreateId: "",
    Name: "",
    Size: "",
    SugarLevel: "",
    IceLevel: "",
    Price: 0,
    Quantity: 0,
    Memo: ""
}

var Model = kendo.data.Model.define({
    id: "Id",
    fields: {
        "Id": { editable: false, nullable: true },
        "StoreName": { type: "string" },
        "StorePhone": { type: "string" },
        "StoreAddress": { type: "string" },
        "DefaultImageId": { type: "string" },
        "EndDate": { type: "datetime" },
        "Note": { type: "string" },
        "OrderDetail": detail
    }
});

function setVisiblePage(p1, p2, p3, p4, p5) {
    viewModel.set('storeVisible', p1); // 店家開團頁面
    viewModel.set('storeAddVisible', p2); // 選定店家設定資料頁面
    viewModel.set('groupVisible', p3); // 飲料團購頁面
    viewModel.set('groupAddVisible', p4); // 飲料團購輸入頁面
    viewModel.set('groupViewVisible', p5); // 飲料團購輸入頁面
}

var viewModel = kendo.observable({
    endDate: null,
    storeId: null,
    note: '',
    detailInfo: '',
    recordByOrderer: '',
    showMenu: false,
    storeVisible: false,
    storeAddVisible: false,
    groupVisible: true,
    groupAddVisible: false,
    groupViewVisible: false,
    openClick: function (e)
    {
        setVisiblePage(false, true, false, false, false);
        viewModel.set('storeId', parseInt(e.currentTarget.previousElementSibling.value));
    },
    backClick: function ()
    {
        setVisiblePage(true, false, false, false, false);
    },
    saveClick: function () {
        if (isValid()) {
            $.ajax({
                url: "/Order/Create",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: JSON.stringify({ "orderVM": { "endDate": viewModel.endDate, "note": viewModel.note }, "storeId": viewModel.storeId })
            }).done(function (response) {
                if (!response.errors) {
                    alert('儲存成功');
                    setVisiblePage(false, false, true, false, false);
                    viewModel.orderDataSource.read();
                }
                else {
                    alert('儲存失敗');
                }
            });
        }
    },
    backGroupClick: function ()
    {
        setVisiblePage(false, false, true, false, false);
    },
    addClick: function ()
    {
        setVisiblePage(true, false, false, false, false);
    },
    viewClick: function (e)
    {
        this.set('recordByOrderer', '');
        this.viewDataSource.data(e.data.OrderDetails);
        this.set('detailInfo', String.format('店家名稱: {0}　電話: {1}　住址: {2}　總額:{3}元'
            , e.data.StoreName, e.data.StorePhone, e.data.StoreAddress, calTotal(e.data.OrderDetails)));
        setVisiblePage(false, false, false, false, true);
    },
    deleteClick: function(e)
    {
        var isDelete = confirm('確定刪除幻飲旅團?');
        if (isDelete) {
            $.ajax({
                url: "/Order/Delete",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: JSON.stringify({ "id": e.data.Id })
            }).done(function (response) {
                if (!response.errors) {
                    alert('刪除成功');
                    viewModel.orderDataSource.remove(e.data);
                }
                else {
                    alert('刪除失敗');
                }
            });
        }
    },
    followClick: function (e) {
        setVisiblePage(false, false, false, true, false);
        $.ajax({
            url: "/Order/GetOrderByUserId?id=" + e.data.Id,
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (response) {
            if (!response.errors) {
                viewModel.detailDataSource.data(response.OrderDetails);
                viewModel.newRowClick();
                viewModel.set('model', e.data);
            }
            else {
                alert('讀取失敗');
            }
        });
    },
    menuClick: function () {
        this.set('showMenu', !this.showMenu);
    },
    newRowClick: function () {
        detail = {
            OrderId: 0,
            CreateId: "",
            Name: "",
            Size: "",
            SugarLevel: "",
            IceLevel: "",
            Price: 0,
            Quantity: 0,
            Memo: ""
        }
        this.detailDataSource.data().push(detail);
    },
    cancelClick: function (e)
    {
        var isCancel = confirm('確定取消?');
        if (isCancel) {
            $.ajax({
                url: "/Order/Cancel",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: JSON.stringify({ "id": e.data.Id })
            }).done(function (response) {
                if (!response.errors) {
                    alert('取消成功');
                    e.data.set('HasDetail', false);
                }
                else {
                    alert('取消失敗');
                }
            });
        }
    },
    deleteRowClick: function (e) {
        this.detailDataSource.remove(e.data);
    },
    exportClick: function(e) {
        if(viewModel.viewDataSource.data().length > 0){
            var orderId = viewModel.viewDataSource.data()[0].OrderId;
            $.ajax({
                url: "/Order/ExportToCsv",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: JSON.stringify({ "id": orderId })
            }).done(function (response) {
                if (!response.errors) {
                    alert('匯出成功');
                }
                else {
                    alert('匯出失敗');
                }
            });
        } else {
            alert('該飲料團無資料，請[返回團購列表]，[跟團]後再進行匯出');
        }
    },
    payClick: function (e) {
        e.target.innerText = '已付款';
        alert('付款完成(無使用資料庫作記錄)');
    },
    saveDetailClick: function () {
        if(isValidDetail()){
            viewModel.set('model.OrderDetails', []);
            $.each(viewModel.detailDataSource.data(), function (index, item) {
                detail.OrderId = viewModel.model.Id;
                detail.CreateId = item.CreateId;
                detail.Name = item.Name;
                detail.Size = item.Size;
                detail.SugarLevel = item.SugarLevel;
                detail.IceLevel = item.IceLevel;
                detail.Price = item.Price;
                detail.Quantity = item.Quantity;
                detail.Memo = item.Memo;
                viewModel.model.OrderDetails.push(detail);
            });
            $.ajax({
                url: "/Order/Save",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                dataType: "json",
                data: kendo.stringify(viewModel.model)
            }).done(function (response) {
                if (!response.errors) {
                    alert('儲存成功');
                    viewModel.orderDataSource.read();
                    viewModel.backGroupClick();
                }
                else {
                    alert('儲存失敗');
                }
            });
        }
    },
    orderDataSource: new kendo.data.DataSource({
        batch: true,
        transport: {
            read: {
                url: "/Order/List",
                dataType: "json"
            }
        }
    }),
    detailDataSource: new kendo.data.DataSource({
        batch: true,
        schema: {
            model: {
                id: "id",
                fields: {
                    Name: { type: "string" },
                    Size: { type: "string" },
                    SugarLevel: { type: "string" },
                    IceLevel: { type: "string" },
                    Price: { type: "number" },
                    Quantity: { type: "number" },
                    Memo: { type: "string" },
                }
            }
        }
    }),
    viewDataSource: new kendo.data.DataSource({
    }),
    model: new Model()
});

$(function () {
    $("#datetimepicker").kendoDateTimePicker({
        format: "yyyy/MM/dd HH:mm",
        min: new Date()
    });

    kendo.bind($(".body-content"), viewModel);
});

$(window).on('load', function () {
    var grid = $('#grid').data('kendoGrid');

    grid.table.on('keydown', function (e) {
        if (e.keyCode === kendo.keys.TAB && $($(e.target).closest('.k-edit-cell'))[0]) {
            e.preventDefault();
            var currentNumberOfItems = grid.columns.length;
            var row = $(e.target).closest('tr').index();
            var col = grid.cellIndex($(e.target).closest('td'));

            var dataItem = grid.dataItem($(e.target).closest('tr'));
            var field = grid.columns[col].field;
            var value = $(e.target).val();
            dataItem.set(field, value);

            if (row >= 0 && row < grid.dataSource.view().length && col >= 0 && col < grid.columns.length) {
                var nextCellRow = row;
                var nextCellCol = col;

                if (nextCellCol + 1 >= currentNumberOfItems) {
                    nextCellRow++;
                    nextCellCol = 1;
                } else {
                    nextCellCol++;
                }

                //由上而下跑 Code都要改 col 改成 row
                //if (nextCellCol >= grid.columns.length || nextCellCol < 0) {
                //    return;
                //}
                if (nextCellRow >= grid.dataSource.view().length || nextCellRow < 0) {
                    return;
                }

                // 指定grid哪一個位置
                setTimeout(function () {
                    grid.editCell(grid.tbody.find("tr:eq(" + nextCellRow + ") td:eq(" + nextCellCol + ")"));
                });
            }
        }
    });
});

function isValid() {
    var errorMsg = '';
    if (!viewModel.endDate) {
        errorMsg = '(截止時間)請輸入有效的日期格式 yyyy/MM/dd HH:mm\n';
    }
    if (errorMsg.length > 0) {
        alert(errorMsg);
    }
    return !!viewModel.endDate;
}

function isValidDetail() {
    var gridRowData = viewModel.detailDataSource.data();
    var errorMsg = '';
    $.each(gridRowData, function (index, row) {
        var str = '';
        str += isRequired(row.Name) ? '' : '飲料名稱、';
        str += isRequired(row.Size) ? '' : '容量、';
        str += isRequired(row.SugarLevel) ? '' : '甜度、';
        str += isRequired(row.IceLevel) ? '' : '冰量、';
        str += row.Price > 0 ? '' : '價格、';
        str += row.Quantity > 0 ? '' : '數量、';

        if (str.length > 0) {
            errorMsg = errorMsg + String.format('第{0}列:請輸入 {1} (共{2}項)\n',
                                            index + 1, str.substring(0, str.length - 1), str.split('、').length - 1);
        }
    });
    if (errorMsg.length > 0) {
        alert(errorMsg);
        return false;
    }
    return true;
}

var calTotal = function (details)
{
    var total = 0;
    $.each(details, function (index, item) {
        if (item.Price && item.Quantity) {
            total += (item.Price * item.Quantity);
        }
        else {
            total += 0;
        }
    });
    return total;
}