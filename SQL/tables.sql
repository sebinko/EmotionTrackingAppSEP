create table users(
    id serial primary key,
    username varchar(16) not null unique,
    password varchar(16) not null,
    email varchar(30) not null unique,
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
    emotion varchar(255) not null,
    created_at timestamp default current_timestamp,
    updated_at timestamp default current_timestamp,
    user_id int,
    foreign key (user_id) references users(id) on delete cascade
);

create table reactions(
    user_id int,
    emotion_id int,
    emoji varchar(255) not null,
    created_at timestamp default current_timestamp,
    updated_at timestamp default current_timestamp,
    primary key (user_id, emotion_id),
    foreign key (user_id) references users(id) on delete cascade,
    foreign key (emotion_id) references emotion_checkins(id) on delete cascade
);

create table tags(
    id serial primary key,
    key varchar(255) not null,
    created_at timestamp default current_timestamp,
    updated_at timestamp default current_timestamp,
    user_id int,
    foreign key (user_id) references users(id) on delete cascade
);

create table tag_emotions(
    emotion_id int,
    tag_id int,
    primary key (emotion_id, tag_id),
    foreign key (emotion_id) references emotion_checkins(id) on delete cascade,
    foreign key (tag_id) references tags(id) on delete cascade
);
