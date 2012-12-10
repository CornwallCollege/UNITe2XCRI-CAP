using System;
using XCRI;
using System.Data;
using System.Data.OracleClient;
using XCRI.Vocabularies.XCRICAP11.Terms;
using AttendancePattern = XCRI.AttendancePattern;
//using Description = XCRI.Vocabularies.XCRICAP11.Terms.Description;
using Description = XCRI.Description;
using System.Configuration;
using System.Net;
/*
catalogDescription,
providerDescription
providerImage
providerIdentifier
providerLocationAddress
providerLocationEmail
providerLocationFax
providerLocationPhone
providerLocationPostcode
providerLocationTown
providerLocationURL
providerTitle        */

namespace XCRIBuilder
{
    class CornwallCollegeProvider:XCRI.Provider
    {

        private DataView _dvAllMasterCourses;
        private DataView _dvAllCourseOfferings;
        private DataView _dvAllQualifications;
        private DataView _dvAllCredits;
        private DataView _dvAllCourseVenues;

        public CornwallCollegeProvider():base()
        {
            this.Url = new Uri(CheckField("","url","providerURL").ToString());
            this.Titles.Add(new XCRI.Title() {Value = CheckField("","string","providerTitle").ToString()});
            this.ReferenceNumber = (long)CheckField("","int","providerReferenceNumber");
            this.Locations.Add(new XCRI.Location()
                {
                    Street = "Trevenson Road",
                    Town = "Pool",
                    Postcode = "TR15 3RD",
                    PhoneNumber = "0845 2232567",
                    EmailAddress = "enquiries@cornwall.ac.uk"
                });
            this.Identifiers.Add(new Identifier() {Value = "https://www.cornwall.ac.uk"});
            this.Descriptions.Add(new XCRI.Description()
                {
                    Value = "The Cornwall College Group is committed to enriching the lives and creating new opportunities for all members of the Cornish community. This is reflected in our mission statement: “To provide learners with a high quality experience, celebrated through vocational and academic achievement, personal development and employability.”"
                });
            
            PopulateDataViews();
            
            foreach (DataRowView dr in _dvAllMasterCourses)
            {
                Course course = AddCourse(dr);

                _dvAllCourseOfferings.RowFilter = "CourseMasterID = " + dr["MasterCourseID"].ToString();
                _dvAllCredits.RowFilter = "MasterCourseID = " + dr["MasterCourseID"].ToString();
                _dvAllQualifications.RowFilter = "MasterCourseID = " + dr["MasterCourseID"].ToString();

                foreach (DataRowView drCredits in _dvAllCredits)
                {
                    course.Credits.Add(AddCredit(drCredits));
                }

                foreach(DataRowView drCourseOffering in _dvAllCourseOfferings)
                {
                    course.Presentations.Add(AddPresentation(drCourseOffering));
                }

                foreach (DataRowView dvAllQualification in _dvAllQualifications)
                {
                    course.Qualifications.Add(AddQualification(dvAllQualification));
                }

                this.Courses.Add(course);
            }
        }
        
        private XCRI.Qualification AddQualification(DataRowView drQualification)
        {
            /*

            qualificationDescriptionProfessionalStatus

            */
            XCRI.Qualification qualification = new Qualification();

            qualification.Abbreviation = new Abbreviation()
                {
                    Value = CheckField(drQualification["abbr"].ToString(), "string", "qualificationAbbr").ToString()
                };

            qualification.AccreditedBy.Add(new QualificationAccreditedBy()
                {
                    Value =
                        CheckField(drQualification["accreditedBy"].ToString(), "string", "qualificationAccreditedBy")
                                               .ToString()
                });

            qualification.AwardedBy.Add(new QualificationAwardedBy()
                {
                    Value =
                        CheckField(drQualification["awardedBy"].ToString(), "string", "qualificationAwardedBy")
                                            .ToString()
                });

            qualification.Descriptions.Add(new XCRI.Description()
                {
                    Value =
                        CheckField(drQualification["description"].ToString(), "string", "qualificationDescription")
                                               .ToString()
                });

            qualification.EducationLevel = new EducationLevel()
                {
                    Value =
                        CheckField(drQualification["educationLevel"].ToString(), "string", "qualificationEducationLevel")
                            .ToString()
                };

            qualification.Identifiers.Add(new Identifier()
                {
                    Value =
                        CheckField(drQualification["identifier"].ToString(), "string", "qualificationIdentifier")
                                              .ToString()
                });

            qualification.Titles.Add(new Title()
                {
                    Value = CheckField(drQualification["title"].ToString(), "string", "qualificationTitle").ToString()
                });

            qualification.Url =
                new Uri(CheckField(drQualification["url"].ToString(), "url", "qualificationUrl").ToString());

            return qualification;

        }

        private XCRI.Venue AddVenue(DataRowView drVenue)
        {
            
            XCRI.Venue venue = new Venue();

            venue.Identifiers.Add(new Identifier()
                {
                    Value =
                        CheckField(drVenue["identifier"].ToString(), "string", "presentationVenueProviderIdentifier")
                                      .ToString()
                });

            venue.Descriptions.Add(new Description()
                {
                    Value =
                        CheckField(drVenue["description"].ToString(), "string", "presentationVenueProviderDescription")
                                       .ToString()
                });

            XCRI.Location _location = new Location();

            _location.Street =
                CheckField(drVenue["address"].ToString(), "string", "presentationVenueProviderLocationAddress")
                    .ToString();

            _location.Postcode =
                CheckField(drVenue["postcode"].ToString(), "string", "presentationVenueProviderLocationPostcode")
                    .ToString();

            _location.EmailAddress = "enquiries@cornwall.ac.uk";

            _location.PhoneNumber =
                CheckField(drVenue["phone"].ToString(), "string", "presentationVenueProviderLocationPhone").ToString();

            venue.Location = _location;

            venue.Titles.Add(new Title()
                {
                    Value =
                        CheckField(drVenue["title"].ToString(), "string", "presentationVenueProviderTitle").ToString()
                });

            return venue;
        }

        private XCRI.Credit AddCredit(DataRowView drCredit)
        {
            XCRI.Credit credit = new Credit();

            credit.Levels.Add(CheckField(drCredit["level"].ToString(), "string", "creditLevel").ToString());
            
            credit.Schemes.Add(CheckField(drCredit["scheme"].ToString(), "string", "creditScheme", 255).ToString());
            
            credit.Values.Add(CheckField(drCredit["value"].ToString(), "int", "creditValue").ToString());

            return credit;

        }

        private XCRI.Course AddCourse(DataRowView drMasterCourse)
        {
            XCRI.Course course = new Course();

            course.Abstracts.Add(new Abstract()
                {
                    Value =
                        CheckField(drMasterCourse["abstract"].ToString(), "string", "courseAbstract", 140).ToString()
                });

            course.Assessments.Add(new Assessment()
                {
                    Value = CheckField(drMasterCourse["assessment"].ToString(), "string", "courseAssessment").ToString()
                });

            course.Descriptions.Add(new Description()
                { ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                   XsiTypeValue = "xcri12terms:careerOutcome",
                    Value =
                        CheckField(drMasterCourse["careerOutcome"].ToString(), "string", "courseCareerOutcome")
                                        .ToString()
                });

            course.Descriptions.Add(new Description()
                {
                    Value =
                        CheckField(drMasterCourse["description"].ToString(), "string", "courseDescription").ToString()
                });

            course.Identifiers.Add(new Identifier()
                {
                    Value = CheckField(drMasterCourse["identifier"].ToString(), "string", "courseIdentifier").ToString()
                });

            course.Descriptions.Add(new Description()
                {ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:indicativeResource",
                    Value =
                        CheckField(drMasterCourse["indicativeResource"].ToString(), "string", "courseIndicativeResource")
                                        .ToString()
                });

            course.Descriptions.Add(new Description()
                {ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:leadsTo",
                    Value = CheckField(drMasterCourse["leadsTo"].ToString(), "string", "courseLeadsTo").ToString()
                });

            course.LearningOutcomes.Add(new LearningOutcome()
                {
                    Value =
                        CheckField(drMasterCourse["learningOutcome"].ToString(), "string", "courseLearningOutcome")
                                            .ToString()
                });


            course.Objectives.Add(new Objective()
                {
                    Value = CheckField(drMasterCourse["objective"].ToString(), "string", "courseObjective").ToString()
                });

            course.Descriptions.Add(new Description()
                {ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:policy",
                    Value = CheckField(drMasterCourse["policy"].ToString(), "string", "coursePolicy").ToString()
                });

            course.Prerequisites.Add(new Prerequisite()
                {
                    Value =
                        CheckField(drMasterCourse["prerequisite"].ToString(), "string", "coursePrerequsite").ToString()
                });

            course.Descriptions.Add(new Description()
                {ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:structure",
                    Value = CheckField(drMasterCourse["structure"].ToString(), "string", "courseStructure").ToString()
                });

            course.Subjects.Add(new Subject()
                {
                    Value = CheckField(drMasterCourse["subject"].ToString(), "string", "courseSubject", 100).ToString()
                });

            course.Descriptions.Add(new Description()
                {ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:support",
                    Value = CheckField(drMasterCourse["support"].ToString(), "string", "courseSupport").ToString()
                });

            course.Titles.Add(new Title()
                {
                    Value = CheckField(drMasterCourse["title"].ToString(), "string", "courseTitle", 255).ToString()
                });

            course.Descriptions.Add(new Description()
                {
                    ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:topic",
                    Value = CheckField(drMasterCourse["topic"].ToString(), "string", "courseTopic").ToString()
                });

            course.Types.Add(new XCRI.Type()
                {
                    Value = CheckField(drMasterCourse["type"].ToString(), "string", "courseType", 50).ToString()
                });


            course.Url = new Uri(CheckField(drMasterCourse["url"].ToString(), "url", "courseUrl").ToString());

            return course;
        }

        private XCRI.Presentation AddPresentation(DataRowView drCourseOffering)
        {
            XCRI.Presentation presentation = new Presentation();

            _dvAllCourseVenues.RowFilter = "CourseOfferingID = " + drCourseOffering["CourseOfferingID"].ToString();

            presentation.Identifiers.Add(new Identifier()
                {
                    Value =
                        CheckField(drCourseOffering["identifier"].ToString(), "string", "presentationIdentifier")
                                             .ToString()
                });

            presentation.Titles.Add(new Title()
                {
                    Value =
                        CheckField(drCourseOffering["title"].ToString(), "string", "presentationTitle", 255).ToString()
                });

            presentation.Url =
                new Uri(CheckField(drCourseOffering["url"].ToString(), "url", "presentationUrl").ToString());

            presentation.Abstracts.Add(new Abstract()
                {
                    Value =
                        CheckField(drCourseOffering["abstract"].ToString(), "string", "presentationAbstract", 140)
                                           .ToString()
                });

            presentation.ApplicationProcedures.Add(new ApplicationProcedure()
                {
                    Value =
                        CheckField(drCourseOffering["applicationProcedure"].ToString(), "string",
                                   "presentationApplicationProcedure").ToString()
                });

            presentation.Assessments.Add(new Assessment()
                {
                    Value =
                        CheckField(drCourseOffering["assessment"].ToString(), "string", "presentationAssessment")
                                             .ToString()
                });

            presentation.Start =
                new XCRI.Date(
                    Convert.ToDateTime(CheckField(drCourseOffering["start"].ToString(), "datetime", "presentationStart")));

            presentation.End =
                new XCRI.Date(
                    Convert.ToDateTime(
                        CheckField(drCourseOffering["end"].ToString(), "datetime", "presentationEnd").ToString()));

            presentation.ApplyUntil =
                new XCRI.Date(
                    Convert.ToDateTime(CheckField(drCourseOffering["applyUntil"].ToString(), "datetime",
                                                  "presentationApplyUntil")));

            presentation.ApplyTo =
                CheckField(drCourseOffering["applyTo"].ToString(), "string", "presentationApplyTo").ToString();

            presentation.Descriptions.Add(new Description()
                {
                   ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:events",
                    Value =
                        (CheckField(drCourseOffering["events"].ToString(), "string", "presentationEvents").ToString())
                }
                );

            presentation.StudyMode = new XCRI.StudyMode()
                {
                    Value =
                        CheckField(drCourseOffering["studyMode"].ToString(), "string", "presentationStudyMode")
                            .ToString()
                };

            presentation.AttendanceMode = new XCRI.AttendanceMode()
                {
                    Value =
                        CheckField(drCourseOffering["attendanceMode"].ToString(), "string", "presentationAttendanceMode")
                            .ToString()
                };

            presentation.AttendancePattern = new AttendancePattern()
                {
                    Value =
                        CheckField(drCourseOffering["attendancePattern"].ToString(), "string",
                                   "presentationAttendancePattern").ToString()
                };

            presentation.PlacesAvailable = CheckField(drCourseOffering["places"].ToString(), "string",
                                                      "presentationPlaces").ToString();

            presentation.Cost = CheckField(drCourseOffering["cost"].ToString(), "string", "presentationCost").ToString();

            presentation.AgeRange =
                CheckField(drCourseOffering["age"].ToString(), "string", "presentationAge").ToString();

            foreach (DataRowView drVenue in _dvAllCourseVenues)
            {
                presentation.Venues.Add(AddVenue(drVenue));
            }


            //TODO: duration

            // presentation.Duration =
            //     (TimeSpan) CheckField(drCourseOffering["duration"].ToString(), "string", "presentationDuration");

            presentation.Descriptions.Add(new Description()
                {
                    ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:providedResource",
                    Value =
                        CheckField(drCourseOffering["providedResource"].ToString(), "string",
                                   "presentationProvidedResource").ToString()
                }
                );

            presentation.Descriptions.Add(new Description()
                {
                    ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:requiredResource",
                    Value = CheckField(drCourseOffering["requiredResource"].ToString(), "string",
                                       "presentationRequiredResource").ToString()
                }
                );

            presentation.Descriptions.Add(new Description()
                {
                    ContentType = XCRI.Interfaces.DescriptionContentTypes.Text,
                     XsiTypeValue = "xcri12terms:studyHours",
                    Value =
                        CheckField(drCourseOffering["studyHours"].ToString(), "string", "presentationStudyHours")
                                              .ToString()
                });

            return presentation;

        }

        private Object CheckField(Object _value, String _type, String _configFieldName,
                                  int optionalLengthOfString = 4000)
        {

            try
            {
                switch (_type)
                {
                    case "string":
                        if (_value == null || _value == "")
                        {
                            return ConfigurationManager.AppSettings[_configFieldName].ToString();
                        }
                        else
                        {
                            if (_value.ToString().Length > optionalLengthOfString)
                            {
                                return SortHTML(_value.ToString().Substring(0, optionalLengthOfString));
                            }
                            else
                            {
                               return  SortHTML(_value.ToString());
                            }
                        }
                    case "url":
                        if (_value == null || _value == "")
                        {
                            return ConfigurationManager.AppSettings[_configFieldName].ToString();
                        }
                        else
                        {
                            if (_value.ToString().Length > optionalLengthOfString)
                            {
                                return _value.ToString().Substring(0, optionalLengthOfString);
                            }
                            else
                            {
                                return _value.ToString();
                            }
                        }

                    case "datetime":
                        if (_value == null || _value == "")
                        {
                            return Convert.ToDateTime(ConfigurationManager.AppSettings[_configFieldName].ToString());
                        }
                        else
                        {
                            return Convert.ToDateTime(_value);
                        }


                    case "int":
                        if (_value == null || _value == "")
                        {
                            return Convert.ToInt64(ConfigurationManager.AppSettings[_configFieldName].ToString());
                        }
                        else
                        {
                            return Convert.ToInt64(_value);
                        }
                    default:
                        return _value.ToString();

                }
            }

            catch (Exception)
            {
                switch (_type)
                {
                    case "string":
                        return "Unavailable";
                    case "datetime":
                        return Convert.ToDateTime("31 July 2013");
                    case "int":
                        return 0;
                    case "url":
                        return "https://www.cornwall.ac.uk";
                    default:
                        return null;
                }
            }
        }

        private String SortHTML(String _string)
        {
            String output;
            const string quote = "\"";
            output = _string;
            for (int i = 0; i < 2; i++)
            {
                output = WebUtility.HtmlDecode(output);
            }
            if (output != _string)
            {
                output = "<![CDATA[<div xmlns=" + quote + "http://www.w3.org/1999/xhtml" + quote + ">" + output + "</div>]]>";
            }

          

            return output;
//                  WebUtility.HtmlDecode(
  ///              WebUtility.HtmlDecode(
     //               WebUtility.HtmlDecode(WebUtility.HtmlDecode(WebUtility.HtmlDecode(WebUtility.HtmlDecode(_string))))));
        }

        private void PopulateDataViews()
        {
            OracleConnection cnn = new OracleConnection("Server=STUDREC;User ID=XCRI;Password=XCRI;");
            
            cnn.Open();

            OracleCommand cmdMasterCourses = new OracleCommand("SELECT * FROM UNITE.IS_XCRI_MASTER_COURSE",cnn);
            OracleDataAdapter daMasterCourses = new OracleDataAdapter(cmdMasterCourses);

            OracleCommand cmdCourseOfferings = new OracleCommand("SELECT * FROM UNITE.IS_XCRI_COURSEOFFERING",cnn);
            OracleDataAdapter daCourseOfferings = new OracleDataAdapter(cmdCourseOfferings);

            OracleCommand cmdQualifications = new OracleCommand("SELECT * FROM UNITE.IS_XCRI_QUALIFICATION",cnn);
            OracleDataAdapter daQualifications = new OracleDataAdapter(cmdQualifications);

            OracleCommand cmdCredits = new OracleCommand("SELECT * FROM UNITE.IS_XCRI_CREDITS",cnn);
            OracleDataAdapter daCredits = new OracleDataAdapter(cmdCredits);

            OracleCommand cmdVenues = new OracleCommand("SELECT * FROM UNITE.IS_XCRI_COURSEVENUES",cnn);
            OracleDataAdapter daVenues = new OracleDataAdapter(cmdVenues);

            DataTable dtMasterCourses = new DataTable();
            daMasterCourses.Fill(dtMasterCourses);
            daMasterCourses.Dispose();
            
            DataTable dtCourseOfferings = new DataTable();
            daCourseOfferings.Fill(dtCourseOfferings);
            daCourseOfferings.Dispose();
            
            DataTable dtQualifications = new DataTable();
            daQualifications.Fill(dtQualifications);
            daQualifications.Dispose();

            DataTable dtCredits = new DataTable();
            daCredits.Fill(dtCredits);
            daCredits.Dispose();

            DataTable dtVenue = new DataTable();
            daVenues.Fill(dtVenue);
            daVenues.Dispose();

            _dvAllMasterCourses = new DataView(dtMasterCourses);
            _dvAllCourseOfferings = new DataView(dtCourseOfferings);
            _dvAllQualifications = new DataView(dtQualifications);
            _dvAllCredits = new DataView(dtCredits);
            _dvAllCourseVenues = new DataView(dtVenue);

            dtMasterCourses.Dispose();
            dtCourseOfferings.Dispose();
            dtQualifications.Dispose();
            dtCredits.Dispose();

            cnn.Close();

        }
  
    }

}
