CREATE OR REPLACE FORCE VIEW "UNITE"."IS_XCRI_CREDITS" ("MasterCourseID", "level", "scheme", "value")
AS
  SELECT DISTINCT t1.m_id "MasterCourseID",
    t2.vc_name "level",
    NULL AS scheme,
    t1.m_creditvalue "value"
  FROM caps_valid_codes t2,
    capd_moduletemplate t6,
    capd_module t3,
    capd_module t1,
    capd_session t4,
    caps_settings t5
  WHERE t2.vc_code(+)      =t1.m_level
  AND t2.vc_domain(+)      ='level'
  AND (t3.m_modulesession  =t4.s_id)
  AND (t4.s_start         >=t5.set_value)
  AND (t5.set_key          ='System\Institution\CurrentAcademicYear\Start')
  AND (t3.m_parent         =t6.mt_id)
  AND (t6.mt_templatemodule=t1.m_id);