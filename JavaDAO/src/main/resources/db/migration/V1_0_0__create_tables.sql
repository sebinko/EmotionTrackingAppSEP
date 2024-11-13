-- SQL GOES HERE HEHE
create table users(
    id serial primary key,
    username varchar not null unique,
    password varchar not null,
    email varchar not null unique,
    created_at timestamp default current_timestamp,
    updated_at timestamp default current_timestamp
);

create table user_friends(
    user_id int,
    friend_id int,
    primary key (user_id, friend_id),
    foreign key (user_id) references users(id) on delete cascade,
    foreign key (friend_id) references users(id) on delete cascade
);

create table emotion_checkins(
    id serial primary key,
    emotion varchar not null,
    created_at timestamp default current_timestamp,
    updated_at timestamp default current_timestamp,
    user_id int,
    foreign key (user_id) references users(id) on delete cascade
);

create table reactions(
    user_id int,
    emotion_checkin_id int,
    emoji varchar not null,
    created_at timestamp default current_timestamp,
    updated_at timestamp default current_timestamp,
    primary key (user_id, emotion_checkin_id),
    foreign key (user_id) references users(id) on delete cascade,
    foreign key (emotion_checkin_id) references emotion_checkins(id) on delete cascade
);

create table tags(
    id serial primary key,
    key varchar not null,
    created_at timestamp default current_timestamp,
    updated_at timestamp default current_timestamp,
    user_id int,
    foreign key (user_id) references users(id) on delete cascade
);

create table tag_emotions(
    emotion_checkin_id int,
    tag_id int,
    primary key (emotion_checkin_id, tag_id),
    foreign key (emotion_checkin_id) references emotion_checkins(id) on delete cascade,
    foreign key (tag_id) references tags(id) on delete cascade
);

with DateDifferences as( select user_id,
    created_at::date as checkin_date,
    created_at::date - (row_number() over (partition by user_id order by created_at::date) * interval '1 day') as streak_group
    from public.emotion_checkins
),

Streaks as(
  select user_id,
    min(checkin_date) as streak_start,
    max(checkin_date) as streak_end,
    count(*) as streak_length
  from DateDifferences
  group by user_id, streak_group
)

select user_id, streak_length
from Streaks
where streak_end = CURRENT_DATE
--in data grid it works!!!
