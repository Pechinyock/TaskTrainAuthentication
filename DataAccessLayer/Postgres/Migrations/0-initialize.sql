
-- create service role
create role tt_auth
with
	login
	password '12345'
	superuser
	createdb
	createrole
	inherit
	replication
	connection limit -1;