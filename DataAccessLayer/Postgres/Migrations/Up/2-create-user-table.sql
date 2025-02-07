create table if not exists public.tt_users
(
   id uuid primary key,
   login varchar(10),
   password_hash varchar(100)
);