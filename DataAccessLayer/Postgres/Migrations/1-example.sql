create database "TTAuth"
with
	owner = tt_auth
	encoding = 'UTF8'
	tablespace = pg_default
	connection limit = -1
	is_template = false;