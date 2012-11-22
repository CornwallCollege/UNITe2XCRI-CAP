CREATE OR REPLACE FORCE VIEW "UNITE"."IS_XCRI_COURSEOFFERING" ("CourseMasterID", "CourseOfferingID", "abstract", "age", "applicationProcedure", "applyTo", "applyUntil", "assessment", "attendanceMode", "attendancePattern", "cost", "duration", "end", "events", "identifier", "places", "providedResource", "requiredResource", "start", "studyMode", "studyHours", "title", "url", "venue")
                                    AS
  SELECT DISTINCT CourseMaster.m_id AS "CourseMasterID",
    CourseOffering.m_id             AS "CourseOfferingID",
    NULL                            AS "abstract",
    NULL                            AS "age",
    NULL                            AS "applicationProcedure",
    'https://www.cornwall.ac.uk/online/offering/'
    || TO_CHAR(CourseOffering.m_reference) AS "applyTo",
    NULL                                   AS "applyUntil",
    NULL                                   AS "assessment",
    NULL                                   AS "attendanceMode",
    NULL                                   AS "attendancePattern",
    courseoffering.m_fee                   AS "cost",
    NULL                                   AS "duration",
    courseoffering.m_end                   AS "end",
    NULL                                   AS "events",
    'https://www.cornwall.ac.uk/online/offering/'
    || TO_CHAR(CourseOffering.m_reference) AS "identifier",
    NULL                                   AS "places",
    NULL                                   AS "providedResource",
    NULL                                   AS "requiredResource",
    courseoffering.m_start                 AS "start",
    NULL                                   AS "studyMode",
    NULL                                   AS "studyHours",
    CourseMaster.m_name                    AS "title",
    'https://www.cornwall.ac.uk/online/offering/'
    || TO_CHAR(CourseOffering.m_reference) AS "url",
    NULL                                   AS "venue"
  FROM capd_session sess,
    capd_module CourseMaster,
    capd_moduletemplate ModuleTemplate,
    capd_moduleisr t7,
    caps_settings t5,
    capd_module CourseOffering
  WHERE t7.mi_id                     =CourseOffering.m_id
  AND (CourseOffering.m_modulesession=sess.s_id)
  AND (t5.set_key LIKE 'System\Institution\CurrentAcademicYear\Start')
  AND (CourseOffering.m_start         >=t5.set_value)
  AND (CourseOffering.m_type           ='CS')
  AND (CourseOffering.m_parent         =ModuleTemplate.mt_id)
  AND (ModuleTemplate.mt_templatemodule=CourseMaster.m_id);