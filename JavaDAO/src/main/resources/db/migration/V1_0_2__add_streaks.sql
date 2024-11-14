CREATE OR REPLACE VIEW users_with_streaks AS
WITH UniqueCheckinDays AS (
    SELECT
        user_id,
        created_at::date AS checkin_date
    FROM
        "EmotionsTrackingWebsite".emotion_checkins
    GROUP BY
        user_id, created_at::date  
),
DateDifferences AS (
    SELECT
        user_id,
        checkin_date,
        checkin_date - (ROW_NUMBER() OVER (PARTITION BY user_id ORDER BY checkin_date) * INTERVAL '1 day') AS streak_group
    FROM
        UniqueCheckinDays
),
Streaks AS (
    SELECT
        user_id,
        MIN(checkin_date) AS streak_start,
        MAX(checkin_date) AS streak_end,
        COUNT(*) AS streak_length
    FROM
        DateDifferences
    GROUP BY
        user_id, streak_group
),
LatestCheckin AS (
    SELECT
        user_id,
        MAX(checkin_date) AS last_checkin_date
    FROM
        UniqueCheckinDays
    GROUP BY
        user_id
)
SELECT
  u.*,
  CASE
    WHEN lc.last_checkin_date = CURRENT_DATE THEN COALESCE(s.streak_length, 0)
    ELSE 0
    END AS current_streak
FROM
  "EmotionsTrackingWebsite".users u
    LEFT JOIN
  Streaks s ON u.id = s.user_id AND s.streak_end = CURRENT_DATE
    LEFT JOIN
  LatestCheckin lc ON u.id = lc.user_id;
