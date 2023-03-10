SELECT * FROM users
WHERE username != 'random'
SELECT user_id FROM users WHERE username = 'random'

INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (1001, 2001, 4001, 4000, 50); SELECT @@IDENTITY

2 WHERE transfer_id = 5015

SELECT * FROM transfer_statuses WHERE account_from = (SELECT account_id FROM accounts WHERE user_id = 3000) OR account_to = (SELECT account_id FROM accounts WHERE user_id = 3000)

SELECT t.transfer_id, t.amount, u.username FROM transfers t JOIN accounts a ON a.account_id = t.account_to JOIN users u ON u.user_id = a.user_id WHERE account_from = (SELECT account_id FROM accounts WHERE user_id = @userId)

SELECT t.transfer_id, t.amount, u.username FROM transfers t JOIN accounts a ON a.account_id = t.account_from JOIN users u ON u.user_id = a.user_id WHERE account_to = (SELECT account_id FROM accounts WHERE user_id = 3000)

SELECT t.transfer_id, tt.transfer_type_desc, ts.transfer_status_desc, t.amount, u.username AS 'From User', v.username AS 'To User' FROM transfers t JOIN accounts a ON a.account_id = t.account_from JOIN accounts b ON b.account_id = t.account_to JOIN users u ON u.user_id = a.user_id JOIN users v ON v.user_id = b.user_id JOIN transfer_statuses ts ON ts.transfer_status_id = t.transfer_status_id JOIN transfer_types tt ON tt.transfer_type_id = t.transfer_type_id WHERE transfer_id = @transferId

INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (1001, 2001, (SELECT account_id FROM accounts WHERE user_id = 3000), (SELECT account_id FROM accounts WHERE user_id = 3001), 100); SELECT @@IDENTITY

INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (1000, 2000, (SELECT account_id FROM accounts WHERE user_id = 3000), (SELECT account_id FROM accounts WHERE user_id = 3001), 100); SELECT @@IDENTITY

SELECT t.transfer_id, t.amount, u.username FROM transfers t JOIN accounts a ON a.account_id = t.account_to JOIN users u ON u.user_id = a.user_id WHERE account_from = (SELECT account_id FROM accounts WHERE user_id = @userId) AND transfer_status_id = 2000