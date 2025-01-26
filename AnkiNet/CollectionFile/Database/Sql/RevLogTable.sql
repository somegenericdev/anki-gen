﻿CREATE TABLE IF NOT EXISTS [revlog]
(
    [id]      integer primary key,
    [cid]     integer not null,
    [usn]     integer not null,
    [ease]    integer not null,
    [ivl]     integer not null,
    [lastIvl] integer not null,
    [factor]  integer not null,
    [time]    integer not null,
    [type]    integer not null
);