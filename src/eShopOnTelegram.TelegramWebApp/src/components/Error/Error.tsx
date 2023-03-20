import React from 'react'
import classes from './Error.module.scss'

interface ErrorProps {
    message: string
}

export default function Error({ message }: ErrorProps) {
  return (
    <div className={classes.errorText}>{message}</div>
  )
}
