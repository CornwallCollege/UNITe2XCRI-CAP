CREATE OR REPLACE FORCE VIEW "UNITE"."IS_XCRI_MASTER_COURSE" ("MasterCourseID", "course", "abstract", "assessment", "careerOutcome", "credit", "description", "identifier", "indicativeResource", "leadsTo", "learningOutcome", "objective", "policy", "prerequisite", "presentation", "qualification", "structure", "subject", "support", "title", "topic", "type", "url")
                                    AS
  SELECT DISTINCT CourseMaster.m_id AS "CourseMasterId",
    NULL                            AS "course",
    NULL                            AS "abstract",
    NULL                            AS "assessment",
    NULL                            AS "careerOutcome",
    NULL                            AS "credit",
    (SELECT LATEDOC.D_TEXT
    FROM IS_LATESTDOCUMENT LATEDOC
    INNER JOIN caps_documents docs
    ON docs.d_id         = LATEDOC.D_ID
    WHERE LATEDOC.D_TYPE ='CourseSum'
    AND docs.d_object_id = CourseMaster.m_id
    ) AS "description",
    'www.cornwall.ac.uk/online/course/'
    || TO_CHAR(CourseMaster.m_reference) AS "identifier",
    NULL                                 AS "indicativeResource",
    NULL                                 AS "leadsTo",
    (SELECT LATEDOC.D_TEXT
    FROM IS_LATESTDOCUMENT LATEDOC
    INNER JOIN caps_documents docs
    ON docs.d_id         = LATEDOC.D_ID
    WHERE LATEDOC.D_TYPE ='Afterwards'
    AND docs.d_object_id = CourseMaster.m_id
    ) AS "learningOutcome",
    (SELECT LATEDOC.D_TEXT
    FROM IS_LATESTDOCUMENT LATEDOC
    INNER JOIN caps_documents docs
    ON docs.d_id         = LATEDOC.D_ID
    WHERE LATEDOC.D_TYPE ='DoOnCourse'
    AND docs.d_object_id = CourseMaster.m_id
    )    AS "objective",
    NULL AS "policy",
    (SELECT LATEDOC.D_TEXT
    FROM IS_LATESTDOCUMENT LATEDOC
    INNER JOIN caps_documents docs
    ON docs.d_id         = LATEDOC.D_ID
    WHERE LATEDOC.D_TYPE ='EntryRequi'
    AND docs.d_object_id = CourseMaster.m_id
    )    AS "prerequisite",
    NULL AS "presentation",
    NULL AS "qualification",
    NULL AS "structure",
    (SELECT LATEDOC.D_TEXT
    FROM IS_LATESTDOCUMENT LATEDOC
    INNER JOIN caps_documents docs
    ON docs.d_id         = LATEDOC.D_ID
    WHERE LATEDOC.D_TYPE ='searchword'
    AND docs.d_object_id = CourseMaster.m_id
    )                            AS "subject",
    'https://www.cornwall.ac.uk' AS "support",
    CourseMaster.m_name          AS "title",
    NULL                         AS "topic",
    CourseMaster.m_type          AS "type",
    'https://www.cornwall.ac.uk/online/course/'
    || TO_CHAR(CourseMaster.m_reference) AS "url"
  FROM capd_session sess,
    capd_module CourseMaster,
    capd_moduletemplate ModuleTemplate,
    capd_moduleisr t7,
    caps_settings t5,
    capd_module CourseOffering,
    caps_valid_codes t23
  WHERE t7.mi_id                     =CourseOffering.m_id
  AND (CourseOffering.m_modulesession=sess.s_id)
  AND (t5.set_key LIKE 'System\Institution\CurrentAcademicYear\Start')
  AND t23.vc_code(+)                   = CourseMaster.m_mode
  AND (CourseOffering.m_start         >=t5.set_value)
  AND (CourseOffering.m_type           ='CS')
  AND (CourseOffering.m_parent         =ModuleTemplate.mt_id)
  AND (ModuleTemplate.mt_templatemodule=CourseMaster.m_id)
  AND CourseOffering.m_onlineenquiry   =-1
  AND CourseMaster.m_onlineenquiry     =-1
  AND CourseOffering.m_status          = 'L'
  AND (CourseOffering.m_start          > sysdate-60
  OR (sess.s_end                       > sysdate
  AND CourseOffering.m_start          IS NULL))
    --Hide A2 and AL
  AND t23.vc_ref NOT  IN ('AL','A2')
  AND t23.vc_domain(+) ='moa'
    --AND ROWNUM           < 25;;