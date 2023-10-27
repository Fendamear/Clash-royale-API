import React from 'react'
import * as AiIcons from "react-icons/ai"
import * as GoIcons from "react-icons/go"
import { VscAccount } from "react-icons/vsc"
import { MdLeaderboard} from "react-icons/md"

export const SidebarData = [
    {
        title: "Home",
        icon: <AiIcons.AiFillHome />,
        link: "/",
        cName: 'nav-text'
    },
    {
        title: "Login",
        icon: <AiIcons.AiFillHome />,
        link: "/login",
        cName: 'nav-text'
    },
    {
        title: "Register",
        icon: <AiIcons.AiFillHome />,
        link: "/register",
        cName: 'nav-text'
    },
    {
        title: "My account",
        icon: <VscAccount color='white' />,
        link: "/Account/" + localStorage.getItem("UserID"),
        cName: 'nav-text'
    },
    {
        title: "F1 Calendar",
        icon: <GoIcons.GoCalendar />,
        link: "/Calendar",
        cName: 'nav-text'
    },
    {
        title: "Standings",
        icon: <MdLeaderboard />,
        link: "Standings",
        cName: 'nav-text'
    },
    {
        title: "Season predictions",
        icon: <GoIcons.GoCalendar />,
        link: "/prediction/season",
        cName: 'nav-text'
    },
    {
        title: "WebSocket",
        icon: <GoIcons.GoCalendar />,
        link: "/websocket",
        cName: 'nav-text'
    },
]