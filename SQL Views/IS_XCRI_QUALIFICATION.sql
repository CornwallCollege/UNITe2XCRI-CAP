CREATE OR REPLACE FORCE VIEW "UNITE"."IS_XCRI_QUALIFICATION" ("MasterCourseID", "abbr", "accreditedBy", "awardedBy", "description", "descriptionprofessionalStatus", "educationLevel", "identifier", "title", "url")
AS
  SELECT DISTINCT t1.m_id "MasterCourseID",
    t2.vc_name "abbr",
    NULL                          AS "accreditedBy",
    t3.AWARDING_ORGANISATION_DESC AS "awardedBy",
    NULL                          AS "description",
    NULL                          AS "descriptionprofessionalStatus",
    NULL                          AS "educationLevel",
    NULL                          AS "identifier",
    NULL                          AS "title",
    NULL                          AS "url"
  FROM capd_moduleisr t8,
    caps_valid_codes t2,
    capd_moduletemplate t5,
    capd_module t1,
    capd_module t4,
    Q_AWARDING_ORGANISATIONS t3,
    capd_session t6,
    LEARNING_AIM t9,
    caps_settings t7
  WHERE t2.vc_code(+)               =t1.m_mode
  AND t2.vc_domain(+)               ='moa'
  AND t8.mi_id(+)                   =t1.m_id
  AND (t4.m_parent                  =t5.mt_id)
  AND (t5.mt_templatemodule         =t1.m_id)
  AND (t4.m_modulesession           =t6.s_id)
  AND (t7.set_key                   ='System\Institution\CurrentAcademicYear\Start')
  AND (t6.s_start                  >=t7.set_value)
  AND (t8.mi_q02m02                 =t9.LEARNING_AIM_REF(+))
  AND (t9.AWARDING_ORGANISATION_CODE=t3.AWARDING_ORGANISATION_CODE(+));