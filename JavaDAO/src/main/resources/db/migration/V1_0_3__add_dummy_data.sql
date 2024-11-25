INSERT INTO "EmotionsTrackingWebsite".users (username, password, email)
VALUES ('jake_peralta', 'l+zR1RiL07YO0fKZhI59G5TNmxniU/RwUxXlDniXXrE=',
        'jake.peralta@nine-nine.com'),
       ('amy_santiago', 'l+zR1RiL07YO0fKZhI59G5TNmxniU/RwUxXlDniXXrE=',
        'amy.santiago@nine-nine.com'),
       ('raymond_holt', 'l+zR1RiL07YO0fKZhI59G5TNmxniU/RwUxXlDniXXrE=',
        'raymond.holt@nine-nine.com'),
       ('rosa_diaz', 'l+zR1RiL07YO0fKZhI59G5TNmxniU/RwUxXlDniXXrE=', 'rosa.diaz@nine-nine.com'),
       ('terry_jeffords', 'l+zR1RiL07YO0fKZhI59G5TNmxniU/RwUxXlDniXXrE=',
        'terry.jeffords@nine-nine.com');

INSERT INTO "EmotionsTrackingWebsite".emotion_checkins (emotion, user_id)
VALUES ('Accomplished', 1),
       ('Excited', 1),
       ('Frustrated', 1),
       ('Alert', 1),
       ('Euphoric', 1);

INSERT INTO "EmotionsTrackingWebsite".emotion_checkins (emotion, user_id)
VALUES ('Confident', 2),
       ('Determined', 2),
       ('At Ease', 2),
       ('Empowered', 2),
       ('Curious', 2);

INSERT INTO "EmotionsTrackingWebsite".emotion_checkins (emotion, user_id)
VALUES ('Calm', 3),
       ('Balanced', 3),
       ('Content', 3),
       ('Even-Tempered', 3),
       ('Empathetic', 3);

INSERT INTO "EmotionsTrackingWebsite".emotion_checkins (emotion, user_id)
VALUES ('Agitated', 4),
       ('Annoyed', 4),
       ('Alienated', 4),
       ('Disgruntled', 4),
       ('Alarmed', 4);

INSERT INTO "EmotionsTrackingWebsite".emotion_checkins (emotion, user_id)
VALUES ('Energized', 5),
       ('Fulfilled', 5),
       ('Cheerful', 5),
       ('Enthusiastic', 5),
       ('Blessed', 5);

INSERT INTO "EmotionsTrackingWebsite".tags (key, user_id)
VALUES
-- Check-in ID 1
('Home', 1),
('Relaxing', 1),

-- Check-in ID 2
('With Friends', 1),

-- Check-in ID 3
('At Work', 1),
('Busy', 1),
('Focused', 1),

-- Check-in ID 4
('Listening to Music', 1),

-- Check-in ID 5
('On a Walk', 1),
('Nature', 1),

-- Check-in ID 6
('Reading', 2),

-- Check-in ID 7
('Gym', 2),
('Exercising', 2),

-- Check-in ID 8
('Planning', 2),

-- Check-in ID 9
('Cooking', 2),
('Family Time', 2),

-- Check-in ID 10
('Celebrating', 2),

-- Check-in ID 11
('Home', 3),
('Quiet', 3),

-- Check-in ID 12
('Team Meeting', 3),

-- Check-in ID 13
('Morning Coffee', 3),
('Reflection', 3),

-- Check-in ID 14
('On Vacation', 3),
('Relaxing', 3),

-- Check-in ID 15
('Studying', 3),

-- Check-in ID 16
('Gaming', 4),
('Online', 4),

-- Check-in ID 17
('Driving', 4),

-- Check-in ID 18
('Exploring', 4),
('Adventure', 4),

-- Check-in ID 19
('With Friends', 4),
('Fun', 4),

-- Check-in ID 20
('Rainy Day', 4),

-- Check-in ID 21
('Beach', 5),
('Sunshine', 5),

-- Check-in ID 22
('Concert', 5),

-- Check-in ID 23
('Family Dinner', 5),
('Laughing', 5),

-- Check-in ID 24
('Working Out', 5),

-- Check-in ID 25
('Evening Walk', 5),
('Alone Time', 5);

INSERT INTO "EmotionsTrackingWebsite".tag_emotions (emotion_checkin_id, tag_id)
VALUES
-- Check-in ID 1 (Emotion: Accomplished, User: Jake Peralta)
(1, 1),   -- Home
(1, 2),   -- Relaxing

-- Check-in ID 2 (Emotion: Excited, User: Jake Peralta)
(2, 3),   -- With Friends

-- Check-in ID 3 (Emotion: Frustrated, User: Jake Peralta)
(3, 4),   -- At Work
(3, 5),   -- Busy
(3, 6),   -- Focused

-- Check-in ID 4 (Emotion: Alert, User: Jake Peralta)
(4, 7),   -- Listening to Music

-- Check-in ID 5 (Emotion: Euphoric, User: Jake Peralta)
(5, 8),   -- On a Walk
(5, 9),   -- Nature

-- Check-in ID 6 (Emotion: Confident, User: Amy Santiago)
(6, 10),  -- Reading

-- Check-in ID 7 (Emotion: Determined, User: Amy Santiago)
(7, 11),  -- Gym
(7, 12),  -- Exercising

-- Check-in ID 8 (Emotion: At Ease, User: Amy Santiago)
(8, 13),  -- Planning

-- Check-in ID 9 (Emotion: Empowered, User: Amy Santiago)
(9, 14),  -- Cooking
(9, 15),  -- Family Time

-- Check-in ID 10 (Emotion: Curious, User: Amy Santiago)
(10, 16), -- Celebrating

-- Check-in ID 11 (Emotion: Calm, User: Raymond Holt)
(11, 17), -- Home
(11, 18), -- Quiet

-- Check-in ID 12 (Emotion: Balanced, User: Raymond Holt)
(12, 19), -- Team Meeting

-- Check-in ID 13 (Emotion: Content, User: Raymond Holt)
(13, 20), -- Morning Coffee
(13, 21), -- Reflection

-- Check-in ID 14 (Emotion: Even-Tempered, User: Raymond Holt)
(14, 22), -- On Vacation
(14, 2),  -- Relaxing

-- Check-in ID 15 (Emotion: Empathetic, User: Raymond Holt)
(15, 23), -- Studying

-- Check-in ID 16 (Emotion: Agitated, User: Rosa Diaz)
(16, 24), -- Gaming
(16, 25), -- Online

-- Check-in ID 17 (Emotion: Alarmed, User: Rosa Diaz)
(17, 26), -- Driving

-- Check-in ID 18 (Emotion: Annoyed, User: Rosa Diaz)
(18, 27), -- Exploring
(18, 28), -- Adventure

-- Check-in ID 19 (Emotion: Alienated, User: Rosa Diaz)
(19, 3),  -- With Friends
(19, 29),
-- Fun

-- Check-in ID 20 (Emotion: Disgruntled, User: Rosa Diaz)
(20, 30),
-- Rainy Day

-- Check-in ID 21 (Emotion: Energized, User: Terry Jeffords)
(21, 31),
-- Beach
(21, 32),
-- Sunshine

-- Check-in ID 22 (Emotion: Fulfilled, User: Terry Jeffords)
(22, 33),
-- Concert

-- Check-in ID 23 (Emotion: Cheerful, User: Terry Jeffords)
(23, 34),
-- Family Dinner
(23, 35),
-- Laughing

-- Check-in ID 24 (Emotion: Enthusiastic, User: Terry Jeffords)
(24, 36),
-- Working Out

-- Check-in ID 25 (Emotion: Blessed, User: Terry Jeffords)
(25, 37)
    ,     -- Evening Walk
(25, 38); -- Alone Time

INSERT INTO "EmotionsTrackingWebsite".user_friends (user_id, friend_id)
VALUES (1, 2), -- Jake Peralta and Amy Santiago
       (1, 3), -- Jake Peralta and Raymond Holt
       (2, 4), -- Amy Santiago and Rosa Diaz
       (2, 5), -- Amy Santiago and Terry Jeffords
       (3, 4), -- Raymond Holt and Rosa Diaz
       (3, 5), -- Raymond Holt and Terry Jeffords
       (4, 1), -- Rosa Diaz and Jake Peralta
       (4, 5), -- Rosa Diaz and Terry Jeffords
       (5, 1), -- Terry Jeffords and Jake Peralta
       (5, 2); -- Terry Jeffords and Amy Santiago

INSERT INTO "EmotionsTrackingWebsite".reactions (user_id, emotion_checkin_id, emoji)
VALUES
-- Jake Peralta reacts to Amy Santiago's check-ins
(1, 6, '👍'),  -- Jake reacts positively to Amy's "Confident"
(1, 7, '🔥'),  -- Jake reacts to Amy's "Determined"

-- Amy Santiago reacts to Jake Peralta's check-ins
(2, 1, '👏'),  -- Amy reacts to Jake's "Accomplished"
(2, 3, '😢'),  -- Amy reacts to Jake's "Frustrated"

-- Raymond Holt reacts to Rosa Diaz's check-ins
(3, 16, '😡'), -- Holt reacts to Rosa's "Agitated"
(3, 19, '🤔'), -- Holt reacts to Rosa's "Alienated"

-- Rosa Diaz reacts to Terry Jeffords's check-ins
(4, 21, '😎'), -- Rosa reacts to Terry's "Energized"
(4, 23, '😂'), -- Rosa reacts to Terry's "Cheerful"

-- Terry Jeffords reacts to Jake Peralta's check-ins
(5, 2, '💯'),  -- Terry reacts to Jake's "Excited"
(5, 4, '😲'),  -- Terry reacts to Jake's "Alert"

-- Cross-user reactions
(3, 9, '🙌'),  -- Holt reacts to Amy's "Empowered"
(4, 14, '🌴'), -- Rosa reacts to Holt's "On Vacation"
(5, 18, '🤯'), -- Terry reacts to Rosa's "Exploring"
(2, 25, '🙏'); -- Amy reacts to Terry's "Blessed"


