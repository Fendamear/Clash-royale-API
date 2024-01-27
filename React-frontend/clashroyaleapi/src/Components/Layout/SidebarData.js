import React from 'react'
import * as AiIcons from "react-icons/ai"
import * as GoIcons from "react-icons/go"
import { VscAccount } from "react-icons/vsc"
import { MdLeaderboard} from "react-icons/md"
import { MdCall } from "react-icons/md";
import { FaPeopleGroup } from "react-icons/fa6";
import { MdStorage } from "react-icons/md";
import { MdLogin } from "react-icons/md";
import { FaRegRegistered } from "react-icons/fa";


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