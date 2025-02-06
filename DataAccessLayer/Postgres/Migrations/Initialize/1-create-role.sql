do
$create_role$
begin
   if exists (
      select from pg_catalog.pg_roles
      where rolname = 'tt_auth_role') then

      raise notice 'role "tt_auth_role" already exists. Skipping.';
   else
      create role tt_auth_role
      with
        login
        password 'qwerty12345'
        superuser
        createdb;
      raise notice 'role "tt_auth_role" successfully created';
   end if;
end
$create_role$;