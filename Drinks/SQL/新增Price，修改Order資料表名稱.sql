use Drinks;

/* 名稱+s */
EXEC sp_rename 'Order' , 'Orders'
EXEC sp_rename 'OrderDetail' , 'OrderDetails'
EXEC sp_rename 'Store' , 'Stores'

/* 新增Price價格 */
ALTER TABLE OrderDetails ADD PRICE int default 0 NOT NULL;