import classes from './MainButton.module.scss'

interface MainButtonProps {
    text: string;
    onClick: () => void;
}

export default function MainButton({ text, onClick }: MainButtonProps) {
  return (
    <div className={classes.wrap}>
      <button className={classes.customMainButton} onClick={onClick}>{text}</button>
    </div>
  )
}
