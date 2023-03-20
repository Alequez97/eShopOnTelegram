import React, { MouseEventHandler } from 'react'
import classes from './Button.module.scss'

interface ButtonProps {
    type: string,
    title: string,
    disabled: boolean,
    onClick: MouseEventHandler<HTMLElement>
}

function Button(props: ButtonProps) {
  return (
    <button 
        className={`${classes.btn} ${
            (props.type === 'add' && `${classes.add}`) || 
            (props.type === 'remove' && `${classes.remove}`) ||
            (props.type === 'checkout' && `${classes.checkout}`)
        }`}
        disabled={props.disabled}
        onClick={props.onClick}
    >
        {props.title}
    </button>
  )
}

export default Button