```sql
-- 7
select customer.id,sum(customerfinance.balance) amout
-- 1
from customer
-- 3
inner join customerfinance 
-- 2
on customer.id = customerfinance.id 
-- 4
where customer.id>305822786048000
-- 5
group by customer.id
-- 6
having sum(customerfinance.balance)>0
-- 8
order by customer.id,amout
-- 9
limit 0,1
```

