create extension if not exists "uuid-ossp";

create table if not exists public.tt_users
(
   id uuid primary key,
   login varchar(10) unique,
   password_hash varchar(100),
   access_layer smallint not null default 0
);

insert into public.tt_users values(uuid_nil()
   , 'admin'
   , 'AQAAAAIAAYagAAAAEG3GBWDNkoxrMVAdb+wOPPnL0xyztcccUT+Y8Lgylg3MpUxjY9KniVhuqg4n/4qMfQ=='
   , 2
);