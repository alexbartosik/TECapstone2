SELECT * FROM users
WHERE username != 'random'
SELECT user_id FROM users WHERE username = 'random'

INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (1001, 2001, 4001, 4000, 50); SELECT @@IDENTITY

SELECT * FROM transfers

SELECT * FROM transfers WHERE account_from = (SELECT account_id FROM accounts WHERE user_id = 3000) OR account_to = (SELECT account_id FROM accounts WHERE user_id = 3000)

