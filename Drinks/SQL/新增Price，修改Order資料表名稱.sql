use Drinks;

/* �W��+s */
EXEC sp_rename 'Order' , 'Orders'
EXEC sp_rename 'OrderDetail' , 'OrderDetails'
EXEC sp_rename 'Store' , 'Stores'

/* �s�WPrice���� */
ALTER TABLE OrderDetails ADD PRICE int default 0 NOT NULL;