SELECT * FROM users
WHERE username != 'random'
SELECT user_id FROM users WHERE username = 'random'

INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (1001, 2001, 4001, 4000, 50); SELECT @@IDENTITY

SELECT * FROM transfers

SELECT * FROM transfers WHERE account_from = (SELECT account_id FROM accounts WHERE user_id = 3000) OR account_to = (SELECT account_id FROM accounts WHERE user_id = 3000)

SELECT t.transfer_id, t.amount, u.username FROM transfers t JOIN accounts a ON a.account_id = t.account_to JOIN users u ON u.user_id = a.user_id WHERE account_from = (SELECT account_id FROM accounts WHERE user_id = @userId)

SELECT t.transfer_id, t.amount, u.username FROM transfers t JOIN accounts a ON a.account_id = t.account_from JOIN users u ON u.user_id = a.user_id WHERE account_to = (SELECT account_id FROM accounts WHERE user_id = 3000)