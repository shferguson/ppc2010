SELECT
r.Recording_file + '.' + Recording_Format as audioFile,
r.Recording_title as title,
r.Recording_dt as recordingDate,
r.recording_speaker_type as speakerTitle,
r.recording_speaker as speakerName,
r.recording_type as recordingSession,
b.Book_Name as book,
r.Recording_Chapter as startChapter,
r.Recording_Start as startVerse,
r.Recording_Chapter as endChapter,
r.Recording_End as endVerse,
r.Recording_Series as sermonSeries
FROM TBL_BibleBooks b , TBL_Recordings r
WHERE r.Recording_Book = b.book_abbrev
order by r.Recording_dt


SELECT
'File.mp3' as audioFile,
r.Recording_title as title,
r.Recording_dt as recordingDate,
r.recording_speaker_type as speakerTitle,
r.recording_speaker as speakerName,
r.recording_type as recordingSession,
b.Book_Name as book,
r.Recording_Chapter as startChapter,
r.Recording_Start as startVerse,
r.Recording_Chapter as endChapter,
r.Recording_End as endVerse,
r.Recording_Series as sermonSeries
FROM TBL_BibleBooks b , TBL_Recordings r
WHERE r.Recording_Book = b.book_abbrev
order by r.Recording_dt

