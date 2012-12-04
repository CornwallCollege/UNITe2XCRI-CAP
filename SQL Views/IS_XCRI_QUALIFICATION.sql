CREATE OR REPLACE FORCE VIEW "UNITE"."IS_XCRI_QUALIFICATION" ("MasterCourseID", "abbr", "accreditedBy", "awardedBy", "description", "descriptionprofessionalStatus", "educationLevel", "identifier", "title", "url")
AS
  SELECT DISTINCT t1.m_id "MasterCourseID",
    t2.vc_name "abbr",
    case when   t1.m_externalaward is null then null else 'Plymouth University' end  AS "accreditedBy",
    t3.AWARDING_ORGANISATION_DESC AS "awardedBy",
    NULL                          AS "description",
    NULL                          AS "descriptionprofessionalStatus",
    t1.m_level                     AS "educationLevel",
    nvl(t8.mi_q02m02,t1.m_externalaward) AS "identifier",
    nvl(t9.learning_aim_title, t1.m_name)AS "title",
    case when t8.mi_q02m02 is not null AND substr(t8.mi_q02m02,1,1) not in ('Z','C')
    then
    'http://register.ofqual.gov.uk/Qualification/Details/'||substr(t8.mi_q02m02,1,3)||'_'||substr(t8.mi_q02m02,4,4)||'_'||substr(t8.mi_q02m02,8,1)
    else
    null end AS "url"
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