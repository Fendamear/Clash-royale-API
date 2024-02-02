import React from 'react'
import * as AiIcons from "react-icons/ai"
import * as GoIcons from "react-icons/go"
import { VscAccount } from "react-icons/vsc"
import { MdLeaderboard } from "react-icons/md"
import { MdCall } from "react-icons/md";
import { FaPeopleGroup } from "react-icons/fa6";
import { MdStorage } from "react-icons/md";
import { MdLogin } from "react-icons/md";
import { FaRegRegistered } from "react-icons/fa";
import { AiOutlineDashboard } from "react-icons/ai";


export const SidebarData = [
  {
    title: "Home",
    icon: <AiIcons.AiFillHome />,
    link: "/",
    cName: 'nav-text'
  },
  {
    title: "Login",
    icon: <MdLogin />,
    link: "/login",
    cName: 'nav-text'
  },
  {
    title: "Register",
    icon: <FaRegRegistered />,
    link: "/register",
    cName: 'nav-text'
  },
  {
    title: "Dashboard",
    icon: <AiOutlineDashboard />,
    link: "/graphs",
    cName: 'nav-text'
  },
  {
    title: "Current river race data",
    icon: <VscAccount color='white' />,
    link: "/currentriverrace",
    cName: 'nav-text'
  },
  {
    title: "Mail Preferences",
    icon: <GoIcons.GoCalendar />,
    link: "/MailSubscription",
    cName: 'nav-text'
  },
  {
    title: "Call List",
    icon: <MdCall />,
    link: "/CallList",
    cName: 'nav-text'
  },
  {
    title: "Clan Members",
    icon: <FaPeopleGroup />,
    link: "/ClanMembers",
    cName: 'nav-text'
  },
  {
    title: "Clan Member Log",
    icon: <MdStorage />,
    link: "/clanMemberLog",
    cName: 'nav-text'
  },
  {
    title: "River Race Log",
    icon: <MdStorage />,
    link: "/riverracelog",
    cName: 'nav-text'
  },
]

export const sectionOptions = [
  {
    value: -1,
    label: "No Filter"
  },
  {
    value: 0,
    label: "Week 1"
  },
  {
    value: 1,
    label: "Week 2"
  }, {
    value: 2,
    label: "Week 3"
  }, {
    value: 3,
    label: "Week 4"
  }, {
    value: 4,
    label: "Week 5"
  }];

export const dayOptions = [
  {
    value: -1,
    label: "No Filter"
  },
  {
    value: 0,
    label: "Thursday"
  },
  {
    value: 1,
    label: "Friday"
  }, {
    value: 2,
    label: "Saturday"
  }, {
    value: 3,
    label: "Sunday"
  }];

export const seasonOptions = [
  {
    value: 105,
    label: "Season 105"
  },
  {
    value: 104,
    label: "Season 104"
  },
  {
    value: 103,
    label: "Season 103"
  },
  {
    value: 102,
    label: "Season 102"
  }];