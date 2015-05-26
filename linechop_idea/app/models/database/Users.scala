package models.database

import play.api.db.slick.Config.driver.simple._
import models.User

class Users extends Table[User]("USER") {

  def id = column[Long]("ID")
  def email = column[String]("EMAIL")
  def passwordHash = column[String]("PASSWORD_HASH")
  def firstName = column[String]("FIRST_NAME")
  def lastName = column[String]("LAST_NAME")
  def phone = column[String]("PHONE")
  def secondaryPhone = column[String]("SECONDARY_PHONE")

  def * = id ~ email ~ passwordHash ~ firstName ~ lastName ~ phone ~ secondaryPhone <> (User.apply _, User.unapply _)

}

