create table if not exists public.tt_users
(
   id uuid primary key,
   login varchar(10) unique,
   password_hash varchar(100),
   access_layer smallint not null default 0
);